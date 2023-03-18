using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Services;
using System;

namespace NSE.WebApp.MVC.Configuration
{
    public static class HttpClientsConfig
    {
        public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAuthService, AuthService>(config =>
            {
                config.BaseAddress = new Uri(configuration.GetValue<string>("AuthenticateUrl"));
            });
        }
    }
}
