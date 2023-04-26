﻿using NSE.Core.Communication;
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

        #region Carrinho

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/shopping/cart/");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<CartViewModel>(response);
        }
        public async Task<int> GetCartAmount()
        {
            var response = await _httpClient.GetAsync("/shopping/cart-amount/");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<int>(response);
        }
        public async Task<ResponseResult> AddCartItem(ItemCartViewModel cart)
        {
            var itemContent = GetContent(cart);

            var response = await _httpClient.PostAsync("/shopping/cart/items/", itemContent);

            if (!HandleErrorResponse(response)) 
                return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
        public async Task<ResponseResult> UpdateCartItem(Guid productId, ItemCartViewModel item)
        {
            var itemContent = GetContent(item);

            var response = await _httpClient.PutAsync($"/shopping/cart/items/{productId}", itemContent);

            if (!HandleErrorResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
        public async Task<ResponseResult> RemoveCartItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/shopping/cart/items/{productId}");

            if (!HandleErrorResponse(response)) 
                return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
        public async Task<ResponseResult> ApplyVoucherCart(string voucher)
        {
            var itemContent = GetContent(voucher);

            var response = await _httpClient.PostAsync("/shopping/cart/apply-voucher/", itemContent);

            if (!HandleErrorResponse(response)) 
                return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
        #endregion
    }
}
