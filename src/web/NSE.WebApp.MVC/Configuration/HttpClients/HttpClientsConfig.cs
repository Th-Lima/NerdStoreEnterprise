using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Services.Identity;
using NSE.WebApp.MVC.Services.Refit.Catalog;
using System;

namespace NSE.WebApp.MVC.Configuration.HttpClients
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

            //services.AddHttpClient<ICatalogService, CatalogService>(config =>
            //{
            //    var catalogUrl = configuration["Settings:CatalogUrl"];

            //    config.BaseAddress = new Uri(catalogUrl);
            //}).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient("Refit", config =>
            {
                var catalogUrl = configuration["Settings:CatalogUrl"];
                config.BaseAddress = new Uri(catalogUrl);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);
        }
    }
}
