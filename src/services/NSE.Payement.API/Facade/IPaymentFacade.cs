using NSE.Payment.API.Models;
using System.Threading.Tasks;

namespace NSE.Payment.API.Facade
{
    public interface IPaymentFacade
    {
        Task<Transaction> AuthorizePayment(Models.Payment payment);
        Task<Transaction> CapturePayment(Transaction transaction);
        Task<Transaction> CancelAuthorization(Transaction transaction);
    }
}
