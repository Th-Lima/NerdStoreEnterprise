using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Catalog.API.Models;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Catalog.API.Services
{
    public class CatalogIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CatalogIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            //Consumo da fila => Consumer
            _bus.SubscribeAsync<OrderAuthorizedIntegrationEvent>("OrderAuthorized", async request =>
                await WithdrawnStock(request));
        }

        public async void CancelOrderWithoutStock(OrderAuthorizedIntegrationEvent message)
        {
            //Publicando mensagem
            //Essa mensagem está sendo ouvida por dois consumers Order e Payment
            var orderCanceled = new OrderCanceledIntegrationEvent(message.CustomerId, message.OrderId);
            await _bus.PublishAsync(orderCanceled);
        }

        private async Task WithdrawnStock(OrderAuthorizedIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productWithStock = new List<Product>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var idsProducts = string.Join(",", message.Itens.Select(c => c.Key));
                var products = await productRepository.GetProductsByIds(idsProducts);

                if (products.Count != message.Itens.Count)
                {
                    CancelOrderWithoutStock(message);
                    return;
                }

                foreach (var product in products)
                {
                    var amountProduct = message.Itens.FirstOrDefault(p => p.Key == product.Id).Value;

                    if (product.IsAvailable(amountProduct))
                    {
                        product.RemoveStock(amountProduct);
                        productWithStock.Add(product);
                    }
                }

                if (productWithStock.Count != message.Itens.Count)
                {
                    CancelOrderWithoutStock(message);
                    return;
                }

                foreach (var product in productWithStock)
                {
                    productRepository.Update(product);
                }

                if (!await productRepository.UnitOfWork.Commit())
                {
                    //WARNING =>
                    //A mensagem irá para uma fila de erros, que depois encaminha novamente para os consumers ...
                    //Então é necessário analisar se lançar exceptions é a melhor alternativa, pois, a execução posterior de mensagens acumuladas, podem causar comportamento indesejados ...
                    //Por exemplo, ao tentar lançar as mensagens novamente, quando voltar a fuincionar, irá vir muitas mensagens de uma vez só, de repente um pedido irá passar a frente do outro, na ordem de quem tem direito ao estoque e etc ...

                    //As vezes a melhor opção é lançar um log de erro, e monitorar, para tratar o problema pontualmente.
                    throw new DomainException($"Problemas ao atualizar estoque do pedido {message.OrderId}");
                }

                //Publicando mensagem que será obtida por um Consumer da Payment API
                var orderWithdrawn = new OrderWithdrawnFromStockIntegrationEvent(message.CustomerId, message.OrderId);
                await _bus.PublishAsync(orderWithdrawn);
            }
        }
    }
}
