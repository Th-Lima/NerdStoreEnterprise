using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Bff.Shopping.Services.gRPC;
using NSE.Cart.API.Services.gRPC;
using System;

namespace NSE.Bff.Shopping.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICartGrpcService, CartGrpcService>();

            services.AddGrpcClient<CartShopping.CartShoppingClient>(options =>
            {
                options.Address = new Uri(configuration["CartUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}
