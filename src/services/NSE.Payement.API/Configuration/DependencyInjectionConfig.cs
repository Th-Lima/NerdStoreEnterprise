using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Payment.API.Data;
using NSE.Payment.API.Data.Repository;
using NSE.Payment.API.Models;
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

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentContext>();
        }
    }
}
