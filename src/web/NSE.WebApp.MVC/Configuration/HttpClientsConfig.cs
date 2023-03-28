using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using System;

namespace NSE.WebApp.MVC.Configuration
{
    public static class HttpClientsConfig
    {
        public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>(config =>
            {
                var authenticateUrl = configuration["Settings:AuthenticateUrl"];

                config.BaseAddress = new Uri(authenticateUrl);
            });

            services.AddHttpClient<ICatalogService, CatalogService>(config =>
            {
                var catalogUrl = configuration["Settings:CatalogUrl"];

                config.BaseAddress = new Uri(catalogUrl);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        }
    }
}
