using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using NSE.Core.Data;

namespace NSE.Payment.API.Models
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void AddPayment(Payment payment);
        void AddTransaction(Transaction transaction);
        Task<Payment> GetPaymentByOrderId(Guid orderId);
        Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId);
    }
}
