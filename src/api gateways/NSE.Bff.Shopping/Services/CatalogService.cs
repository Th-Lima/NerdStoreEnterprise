using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using NSE.Bff.Shopping.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services
{
    public interface ICatalogService
    {
        Task<ItemProductDto> GetById(Guid id);
        Task<IEnumerable<ItemProductDto>> GetItems(IEnumerable<Guid> ids);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        }

        public async Task<ItemProductDto> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<ItemProductDto>(response);
        }

        public async Task<IEnumerable<ItemProductDto>> GetItems(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/catalog/products/list/{idsRequest}/");

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<IEnumerable<ItemProductDto>>(response);
        }
    }
}
