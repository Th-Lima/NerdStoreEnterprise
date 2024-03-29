﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Cart;
using NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Catalog;
using NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Customer;
using NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Identity;
using NSE.WebApp.MVC.Services.Cart;
using NSE.WebApp.MVC.Services.Catalog;
using NSE.WebApp.MVC.Services.Customer;
using NSE.WebApp.MVC.Services.Identity;
using Polly;
using System;

namespace NSE.WebApp.MVC.Configuration.HttpClients
{
    public static class HttpClientsConfig
    {
        public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            var retryWaitIdentityPolicy = RetryPatternPoliciesIdentityApi.HandleRetryPatternIdentityApi();
            services.AddHttpClient<IAuthService, AuthService>(config =>
            {
                var authenticateUrl = configuration["Settings:AuthenticateUrl"];
                 
                config.BaseAddress = new Uri(authenticateUrl);
            }).AddPolicyHandler(retryWaitIdentityPolicy)
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


            var retryWaitCatalogPolicy = RetryPatternPoliciesCatalogApi.HandleRetryPatternCatalogApi();
            services.AddHttpClient<ICatalogService, CatalogService>(config =>
            {
                var catalogUrl = configuration["Settings:CatalogUrl"];

                config.BaseAddress = new Uri(catalogUrl);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(retryWaitCatalogPolicy)
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            var retryWaitCartPolicy = RetryPatternPoliciesShoppingBffApi.HandleRetryPatternShoppingBffApi();
            services.AddHttpClient<IShoppingBffService, ShoppingBffService>(config =>
            {
                var shoppingBffUrl = configuration["Settings:ShoppingBffUrl"];

                config.BaseAddress = new Uri(shoppingBffUrl);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(retryWaitCartPolicy)
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            var retryWaitCustomerPolicy = RetryPatternPoliciesCustomerApi.HandleRetryPatternCustomerApi();
            services.AddHttpClient<ICustomerService, CustomerService>(config =>
            {
                var customerUrl = configuration["Settings:CustomerUrl"];

                config.BaseAddress = new Uri(customerUrl);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(retryWaitCartPolicy)
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            #region Refit
            //UTILIZAÇÃO DO REFIT
            //services.AddHttpClient("Refit", config =>
            //{
            //    var catalogUrl = configuration["Settings:CatalogUrl"];
            //    config.BaseAddress = new Uri(catalogUrl);
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //.AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);
            #endregion
        }
    }
}
