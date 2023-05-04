using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Payment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Payment.API.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public IUnitOfWork UnitOfWork => _context;

        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public async Task<Models.Payment> GetPaymentByOrderId(Guid orderId)
        {
            return await _context.Payments.AsNoTracking()
               .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId)
        {
            return await _context.Transactions.AsNoTracking()
                .Where(t => t.Payment.OrderId == orderId).ToListAsync();
        }

        public void AddPayment(Models.Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }
    
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
