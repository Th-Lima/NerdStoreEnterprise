using NSE.Identity.API.Models;
using System;
using System.Threading.Tasks;

namespace NSE.Identity.API.Services.Interfaces
{
    public interface INseAuthenticationService
    {
        Task<UserIdentityResponseLogin> GenerateJwtAsync(string email);
        Task<RefreshToken> GetRefreshToken(Guid refreshToken);
    }
}
