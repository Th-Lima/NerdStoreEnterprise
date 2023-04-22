using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Threading.Tasks;


namespace NSE.WebApp.MVC.Services.Identity
{
    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> LoginAsync(UserLogin userLogin)
        {
            var loginContent = GetContent(userLogin);

            var response = await _httpClient.PostAsync($"/api/identity/login", loginContent);

            if (!HandleErrorResponse(response))
            {
                return new UserResponseLogin()
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            };

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> RegisterAsync(UserRegister userRegister)
        {
            var registerContent = GetContent(userRegister);

            var response = await _httpClient.PostAsync($"/api/identity/new-account", registerContent);

            if (!HandleErrorResponse(response))
            {
                return new UserResponseLogin()
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            };

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }
    }
}
