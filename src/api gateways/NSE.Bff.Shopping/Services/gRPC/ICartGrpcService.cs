using NSE.Bff.Shopping.Models;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services.gRPC
{
    public interface ICartGrpcService
    {
        Task<CartDto> GetCart();
    }
}