using Microsoft.Extensions.DependencyInjection;
using NSE.Identity.API.Services;
using NSE.Identity.API.Services.Interfaces;

namespace NSE.Identity.API.Configuration
{
    public static class DependancyInjectionConfiguration
    {
        public static IServiceCollection AddDependancyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<INseAuthenticationService, NseAuthenticationService>();

            return services;
        }

    }
}
