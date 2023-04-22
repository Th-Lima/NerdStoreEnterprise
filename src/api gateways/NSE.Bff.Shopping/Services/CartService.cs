using Microsoft.Extensions.Options;
using NSE.Bff.Shopping.Extensions;
using NSE.Bff.Shopping.Models;
using NSE.Core.Communication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCart();
        Task<ResponseResult> AddItemCart(ItemCartDto product);
        Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDto cart);
        Task<ResponseResult> RemoveItemCart(Guid productId);
        Task<ResponseResult> ApplyVoucherCart(VoucherDto voucher);
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
        }

        public async Task<CartDto> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            HandleErrorsResponse(response);

            return await DeserializarObjectResponse<CartDto>(response);
        }

        public async Task<ResponseResult> AddItemCart(ItemCartDto product)
        {
            var itemContent = GetContent(product);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializarObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemCart(Guid productId, ItemCartDto cart)
        {
            var itemContent = GetContent(cart);

            var response = await _httpClient.PutAsync($"/cart/{cart.ProductId}", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializarObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveItemCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!HandleErrorsResponse(response)) return await DeserializarObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ApplyVoucherCart(VoucherDto voucher)
        {
            var itemContent = GetContent(voucher);

            var response = await _httpClient.PostAsync("/cart/apply-voucher/", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializarObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
