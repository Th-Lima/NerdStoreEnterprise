using System;

namespace NSE.Core.Messages.Integration
{
    public class OrderRealizedIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; private set; }

        public OrderRealizedIntegrationEvent(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
