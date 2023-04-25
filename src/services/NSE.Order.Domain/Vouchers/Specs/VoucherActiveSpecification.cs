using NetDevPack.Specification;
using System;
using System.Linq.Expressions;

namespace NSE.Order.Domain.Vouchers.Specs
{
    public   class VoucherActiveSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Active && !voucher.Used;
        }
    }
}
