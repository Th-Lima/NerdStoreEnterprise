using NSE.Core.Messages;
using System;

namespace NSE.Order.API.Application.Events
{
    public class OrderRealizedEvent : Event
    {
        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }

        public OrderRealizedEvent(Guid pedidoId, Guid clienteId)
        {
            OrderId = pedidoId;
            ClientId = clienteId;
        }
    }
}
