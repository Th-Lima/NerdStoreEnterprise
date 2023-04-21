using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Cart;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService carrinhoService)
        {
            _cartService = carrinhoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _cartService.GetCart() ?? new CartViewModel());
        }
    }
}
