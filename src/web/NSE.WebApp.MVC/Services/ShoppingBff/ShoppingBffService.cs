using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
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

        #region Cart

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

        #region Order
        public async Task<ResponseResult> FinalizeOrder(OrderTransactionViewModel orderTransaction)
        {
            var orderContent = GetContent(orderTransaction);

            var response = await _httpClient.PostAsync("/shopping/order/", orderContent);

            if (!HandleErrorResponse(response)) 
                return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<OrderViewModel> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("/shopping/order/last/");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<OrderViewModel>(response);
        }

        public async Task<IEnumerable<OrderViewModel>> GetListByCustomerId()
        {
            var response = await _httpClient.GetAsync("/shopping/order/list-customer/");

            HandleErrorResponse(response);

            return await DeserializeObjectResponse<IEnumerable<OrderViewModel>>(response);
        }

        public OrderTransactionViewModel MapForOrder(CartViewModel cart, AddressViewModel Address)
        {
            var order = new OrderTransactionViewModel
            {
                TotalValue = cart.TotalValue,
                Itens = cart.Itens,
                Discount = cart.Discount,
                VoucherUsed = cart.VoucherUsed,
                VoucherCode = cart.Voucher?.Code
            };

            if (Address != null)
            {
                order.Address = new AddressViewModel
                {
                    AddressPlace = Address.AddressPlace,
                    NumberAddress = Address.NumberAddress,
                    Neighborhood = Address.Neighborhood,
                    ZipCode = Address.ZipCode,
                    Complement = Address.Complement,
                    City = Address.City,
                    State = Address.State
                };
            }

            return order;
        }
        #endregion
    }
}
