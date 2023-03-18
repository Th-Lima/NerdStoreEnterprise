using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
