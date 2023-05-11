using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Catalog
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedViewModel<ProductViewModel>> GetAll(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/catalog/products?pageSize={pageSize}&pageIndex={pageIndex}&query={query}");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<PagedViewModel<ProductViewModel>>(response);
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<ProductViewModel>(response);
        }
    }
}
