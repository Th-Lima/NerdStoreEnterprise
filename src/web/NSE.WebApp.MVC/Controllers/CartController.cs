using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Cart;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly IShoppingBffService _shoppingBffService;

        public CartController(IShoppingBffService shoppingBffService)
        {
            _shoppingBffService = shoppingBffService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _shoppingBffService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddCartItem(ItemCartViewModel itemProductViewModel)
        {
            var response = await _shoppingBffService.AddCartItem(itemProductViewModel);

            if (ResponseHasErrors(response)) return View("Index", await _shoppingBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, int amount)
        {
            var item = new ItemCartViewModel { ProductId = productId, Amount = amount };
            var response = await _shoppingBffService.UpdateCartItem(productId, item);

            if (ResponseHasErrors(response)) return View("Index", await _shoppingBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/remove-item")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var response = await _shoppingBffService.RemoveCartItem(productId);

            if (ResponseHasErrors(response)) return View("Index", await _shoppingBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var response = await _shoppingBffService.ApplyVoucherCart(voucherCode);

            if (ResponseHasErrors(response)) 
                return View("Index", await _shoppingBffService.GetCart());

            return RedirectToAction("Index");
        }
    }
}
