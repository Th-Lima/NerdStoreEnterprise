using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using System;
using System.Net.Http;

namespace NSE.Bff.Shopping.Services
{
    public interface ICatalogService
    {
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        }
    }
}
