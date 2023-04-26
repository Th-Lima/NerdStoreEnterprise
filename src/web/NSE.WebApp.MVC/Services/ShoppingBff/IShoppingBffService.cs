using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Cart
{
    public interface IShoppingBffService
    {
        Task<CartViewModel> GetCart();
        Task<int> GetCartAmount();
        Task<ResponseResult> AddCartItem(ItemCartViewModel product);
        Task<ResponseResult> UpdateCartItem(Guid productId, ItemCartViewModel product);
        Task<ResponseResult> RemoveCartItem(Guid productId);
        Task<ResponseResult> ApplyVoucherCart(string voucher);
    }
}
