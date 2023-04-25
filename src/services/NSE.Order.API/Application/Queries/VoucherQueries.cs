using NSE.Order.API.Application.Dto;
using NSE.Order.Domain.Vouchers;
using System.Threading.Tasks;

namespace NSE.Order.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDto> GetVoucherByCode(string code);
    }

    public class VoucherQueries : IVoucherQueries
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDto> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);

            if (voucher == null)
                return null;

            if (!voucher.IsValidForUtilization())
                return null;

            return new VoucherDto
            {
                Code = voucher.Code,
                DiscountType = (int)voucher.DiscountType,
                Percentage = voucher.Percentage,
                ValueDiscount = voucher.ValueDiscount
            };
        }
    }
}
