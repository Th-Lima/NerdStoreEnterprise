using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebAPI.Core.User;

namespace NSE.Payment.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //services.AddScoped<IPagamentoService, PagamentoService>();
            //services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            //services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            //services.AddScoped<PagamentosContext>();
        }
    }
}
