using NSE.Core.DomainObjects;
using System;

namespace NSE.Order.Domain.Orders
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Amount { get; private set; }
        public decimal UnitValue { get; private set; }
        public string ProductImage { get; set; }

        // EF Rel.
        public Order Order { get; set; }

        public OrderItem(Guid productId, string productName, int amount,
            decimal unitValue, string productImage = null)
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            UnitValue = unitValue;
            ProductImage = productImage;
        }

        // EF ctor
        protected OrderItem() { }

        internal decimal CalculateValue()
        {
            return Amount * UnitValue;
        }
    }
}
