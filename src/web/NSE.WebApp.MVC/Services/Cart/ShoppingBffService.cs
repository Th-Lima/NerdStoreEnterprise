using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Cart
{
    public class ShoppingBffService : Service, IShoppingBffService
    {
        private readonly HttpClient _httpClient;

        public ShoppingBffService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<CartViewModel>(response);
        }

        public async Task<ResponseResult> AddCartItem(ItemProductViewModel product)
        {
            var itemContent = GetContent(product);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!HandleErrorResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateCartItem(Guid productId, ItemProductViewModel product)
        {
            var itemContent = GetContent(product);

            var response = await _httpClient.PutAsync($"/cart/{product.ProductId}", itemContent);

            if (!HandleErrorResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveCartItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!HandleErrorResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
