using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> LoginAsync(UserLogin userLogin);
        Task<UserResponseLogin> RegisterAsync(UserRegister userRegister);
    }
}
