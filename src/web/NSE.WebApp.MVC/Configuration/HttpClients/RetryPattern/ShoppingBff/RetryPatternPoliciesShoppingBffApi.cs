using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Http;
using System;
using Polly;

namespace NSE.WebApp.MVC.Configuration.HttpClients.RetryPattern.Cart
{
    public class RetryPatternPoliciesShoppingBffApi
    {
        public static AsyncRetryPolicy<HttpResponseMessage> HandleRetryPatternShoppingBffApi()
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Tentando pela {retryCount} vez!");
                    Console.ForegroundColor = ConsoleColor.White;
                    //In production environment log would well
                });

            return retryPolicy;
        }
    }
}
