using NetDevPack.Specification;
using NSE.Order.Domain.Vouchers.Specs;

namespace NSE.Order.Domain.Vouchers.Validation
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            var dataSpec = new VoucherDateSpecification();
            var qtdeSpec = new VoucherAmountSpecification();
            var ativoSpec = new VoucherActiveSpecification();

            Add("dateSpec", new Rule<Voucher>(dataSpec, "Este voucher está expirado"));
            Add("amountSpec", new Rule<Voucher>(qtdeSpec, "Este voucher já foi utilizado"));
            Add("activeSpec", new Rule<Voucher>(ativoSpec, "Este voucher não está mais ativo"));
        }
    }
}
