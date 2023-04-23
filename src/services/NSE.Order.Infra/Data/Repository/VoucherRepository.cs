using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Order.Domain.Vouchers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Order.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {

        private readonly OrderContext _context;

        public VoucherRepository(OrderContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.Code == code);
        }

        public void Update(Voucher voucher)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
