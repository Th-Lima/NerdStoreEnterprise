using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Catalog;
using NSE.WebApp.MVC.Services.Catalog;
using NSE.WebApp.MVC.Services.Identity;
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


            var retryWaitPolicy = RetryPatternPoliciesCatalogApi.HandleRetryPatternCatalogApi();

            services.AddHttpClient<ICatalogService, CatalogService>(config =>
            {
                var catalogUrl = configuration["Settings:CatalogUrl"];

                config.BaseAddress = new Uri(catalogUrl);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(retryWaitPolicy);
            //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            //UTILIZAÇÃO DO REFIT
            //services.AddHttpClient("Refit", config =>
            //{
            //    var catalogUrl = configuration["Settings:CatalogUrl"];
            //    config.BaseAddress = new Uri(catalogUrl);
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //.AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);
        }
    }
}
