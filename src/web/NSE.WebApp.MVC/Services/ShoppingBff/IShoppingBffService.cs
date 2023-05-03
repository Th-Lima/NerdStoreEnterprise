using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
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

        //Order
        Task<ResponseResult> FinalizeOrder(OrderTransactionViewModel orderTransaction);
        Task<OrderViewModel> GetLastOrder();
        Task<IEnumerable<OrderViewModel>> GetListByCustomerId();
        OrderTransactionViewModel MapForOrder(CartViewModel cart, AddressViewModel address);
    }
}
