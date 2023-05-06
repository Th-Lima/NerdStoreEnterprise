using NSE.Core.Messages.Integration;
using System;
using System.Threading.Tasks;

namespace NSE.Payment.API.Services
{
    public interface IPaymentService
    {
        Task<ResponseMessage> AuthorizePayment(Models.Payment payment);
        Task<ResponseMessage> CapturePayment(Guid orderId);
        Task<ResponseMessage> CancelPayment(Guid orderId);
    }
}
