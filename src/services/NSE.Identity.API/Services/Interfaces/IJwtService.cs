using NSE.Identity.API.Models;
using System.Threading.Tasks;

namespace NSE.Identity.API.Services.Interfaces
{
    public interface IJwtService
    {
        Task<UserIdentityResponseLogin> GenerateJwtAsync(string email);
    }
}
