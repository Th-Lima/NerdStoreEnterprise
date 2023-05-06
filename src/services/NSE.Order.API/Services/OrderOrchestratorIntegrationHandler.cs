using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Order.API.Application.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Order.API.Services
{
    public class OrderOrchestratorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger<OrderOrchestratorIntegrationHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public OrderOrchestratorIntegrationHandler(ILogger<OrderOrchestratorIntegrationHandler> logger,
                                                   IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos iniciado");

            _timer = new Timer(ProcessOrders, null, TimeSpan.Zero,
               TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos finalizado.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void ProcessOrders(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();
                var order = await orderQueries.GetOrdersAuthorized();

                if (order == null)
                    return;

                var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

                var orderAuthorized = new OrderAuthorizedIntegrationEvent(order.CustomerId, order.Id,
                    order.OrderItems.ToDictionary(p => p.ProductId, p => p.Amount));

                await bus.PublishAsync(orderAuthorized); //Fire, Forget

                _logger.LogInformation($"Pedido ID: {order.Id} foi encaminhado para baixa no estoque.");
            }
        }
    }
}
