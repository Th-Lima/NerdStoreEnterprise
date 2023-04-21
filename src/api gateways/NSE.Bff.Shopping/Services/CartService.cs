using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using System;
using System.Net.Http;

namespace NSE.Bff.Shopping.Services
{
    public interface ICartService
    {
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
        }
    }
}
