using System;

namespace NSE.Core.Messages.Integration
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; set; }
        public Guid OrderId { get; set; }
        public int TypePayment { get; set; }
        public decimal TotalValue { get; set; }

        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string MonthYearDue { get; set; }
        public string CVV { get; set; }
    }
}
