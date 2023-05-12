using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Identity
{
    public interface IAuthService
    {
        Task<UserResponseLogin> LoginAsync(UserLogin userLogin);
        Task<UserResponseLogin> RegisterAsync(UserRegister userRegister);
        Task PerformLogin(UserResponseLogin response);
        Task Logout();
        Task<UserResponseLogin> UseRefreshToken(string refreshToken);
        bool TokenExpired();
        Task<bool> RefreshTokenValid();
    }
}
