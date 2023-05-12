using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.Core.Communication;
using NSE.WebAPI.Core.User;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


namespace NSE.WebApp.MVC.Services.Identity
{
    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _aspNetUser;
        private readonly IAuthenticationService _authenticationService;

        public AuthService(HttpClient httpClient, IAuthenticationService authenticationService, IAspNetUser aspNetUser)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
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

        public async Task<UserResponseLogin> UseRefreshToken(string refreshToken)
        {
            var refreshTokenContent = GetContent(refreshToken);

            var response = await _httpClient.PostAsync("/api/identity/refresh-token", refreshTokenContent);

            if (!HandleErrorResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public static JwtSecurityToken GetTokenFormated(string jwt)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwt) as JwtSecurityToken;
        }

        public async Task PerformLogin(UserResponseLogin response)
        {
            var token = GetTokenFormated(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.Add(new Claim("RefreshToken", response.RefreshToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _aspNetUser.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(_aspNetUser.GetHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
        }

        public bool TokenExpired()
        {
            var jwt = _aspNetUser.GetUserToken();
            if (jwt is null) return false;

            var token = GetTokenFormated(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValid()
        {
            var resposta = await UseRefreshToken(_aspNetUser.GetUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await PerformLogin(resposta);
                return true;
            }

            return false;
        }
    }
}
