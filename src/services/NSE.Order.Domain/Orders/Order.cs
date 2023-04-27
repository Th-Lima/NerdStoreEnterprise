using NSE.Core.DomainObjects;
using NSE.Order.Domain.Vouchers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Order.Domain.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        public Order(Guid clientId, decimal totalValue, List<OrderItem> oerderItems,
            bool voucherUsed = false, decimal discount = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            TotalValue = totalValue;
            _orderItems = oerderItems;

            Discount = discount;
            VoucherUsed = voucherUsed;
            VoucherId = voucherId;
        }

        // EF ctor
        protected Order() { }

        public int Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public DateTime CreationDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Address Address { get; private set; }

        // EF Rel.
        public Voucher Voucher { get; private set; }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorized;
        }

        public void AssignVoucher(Voucher voucher)
        {
            VoucherUsed = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AssignAddress(Address endereco)
        {
            Address = endereco;
        }

        public void CalculateOrderValue()
        {
            TotalValue = OrderItems.Sum(p => p.CalculateValue());
            CalculateTotalValueDiscount();
        }

        public void CalculateTotalValueDiscount()
        {
            if (!VoucherUsed) return;

            decimal desconto = 0;
            var valor = TotalValue;

            if (Voucher.DiscountType == TypeDiscountVoucher.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    desconto = (valor * Voucher.Percentage.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValueDiscount.HasValue)
                {
                    desconto = Voucher.ValueDiscount.Value;
                    valor -= desconto;
                }
            }

            TotalValue = valor < 0 ? 0 : valor;
            Discount = desconto;
        }
    }
}
