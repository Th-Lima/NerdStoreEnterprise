using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;
using System;

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
