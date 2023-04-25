using NetDevPack.Specification;
using System;
using System.Linq.Expressions;

namespace NSE.Order.Domain.Vouchers.Specs
{
    public class VoucherAmountSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Amount > 0;
        }
    }
}
