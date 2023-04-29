using NetDevPack.Specification;
using NSE.Order.Domain.Vouchers.Specs;

namespace NSE.Order.Domain.Vouchers.Validation
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            var dateSpec = new VoucherDateSpecification();
            var amountSpec = new VoucherAmountSpecification();
            var activeSpec = new VoucherActiveSpecification();

            Add("dateSpec", new Rule<Voucher>(dateSpec, "Este voucher está expirado"));
            Add("amountSpec", new Rule<Voucher>(amountSpec, "Este voucher já foi utilizado"));
            Add("activeSpec", new Rule<Voucher>(activeSpec, "Este voucher não está mais ativo"));
        }
    }
}
