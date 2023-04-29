using MediatR;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Order.API.Application.Events
{
    public class OrderEventHandler : INotificationHandler<OrderRealizedEvent>
    {
        private readonly IMessageBus _bus;

        public OrderEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderRealizedEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new OrderRealizedIntegrationEvent(message.ClientId));
        }
    }
}
