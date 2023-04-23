using NSE.Core.DomainObjects;
using System;

namespace NSE.Order.Domain.Vouchers
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? ValueDiscount { get; private set; }
        public int Amount { get; private set; }
        public TypeDiscountVoucher DiscountType { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? UtilizationDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        //public bool IsValidForUtilization()
        //{
        //    return new VoucherAtivoSpecification()
        //        .And(new VoucherDataSpecification())
        //        .And(new VoucherQuantidadeSpecification())
        //        .IsSatisfiedBy(this);
        //}

        public void MarkAsUtilized()
        {
            Active = false;
            Used = true;
            Amount = 0;
            UtilizationDate = DateTime.Now;
        }

        public void DebitAmount()
        {
            Amount -= 1;
            if (Amount >= 1) return;

            MarkAsUtilized();
        }
    }
}
