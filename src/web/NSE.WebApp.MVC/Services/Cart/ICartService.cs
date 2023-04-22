using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Cart
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart();
        Task<ResponseResult> AddCartItem(ItemProductViewModel product);
        Task<ResponseResult> UpdateCartItem(Guid productId, ItemProductViewModel product);
        Task<ResponseResult> RemoveCartItem(Guid productId);
    }
}
