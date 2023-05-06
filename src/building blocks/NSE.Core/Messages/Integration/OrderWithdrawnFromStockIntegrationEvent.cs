using System;

namespace NSE.Core.Messages.Integration
{
    public class OrderWithdrawnFromStockIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public OrderWithdrawnFromStockIntegrationEvent(Guid customerId, Guid orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}
