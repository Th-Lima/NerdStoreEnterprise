using Microsoft.Extensions.DependencyInjection;
using NSE.Bff.Shopping.Extensions;
using NSE.Bff.Shopping.Services;
using NSE.WebAPI.Core.Extensions;
using Polly;
using System;

namespace NSE.Bff.Shopping.Configuration
{
    public static class HttpClientsConfig
    {
        public static void RegisterHttpClients(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.HandleRetryPatternWaitAndRetry())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICartService, CartService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.HandleRetryPatternWaitAndRetry())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        }
    }
}
