using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Domain;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Order.Domain.Orders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Order.API.Services
{
    public class OrderIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public OrderIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        //Consumers
        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderCanceledIntegrationEvent>("OrderCanceled",
                async request => await CancelOrder(request));

            _bus.SubscribeAsync<OrderPaidIntegrationEvent>("OrderPaid",
               async request => await FinalizeOrder(request));
        }

        private async Task CancelOrder(OrderCanceledIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var order = await orderRepository.GetById(message.OrderId);
                order.CancelOrder();

                orderRepository.Update(order);

                if (!await orderRepository.UnitOfWork.Commit())
                    throw new DomainException($"Problemas ao cancelar o pedido {message.OrderId}");
            }
        }

        private async Task FinalizeOrder(OrderPaidIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var order = await orderRepository.GetById(message.OrderId);
                order.FinalizeOrder();

                orderRepository.Update(order);

                if (!await orderRepository.UnitOfWork.Commit())
                    throw new DomainException($"Problemas ao finalizar o pedido {message.OrderId}");
            }
        }
    }
}
