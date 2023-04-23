using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services.Cart;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IShoppingBffService _shoppingBffService;

        public CartViewComponent(IShoppingBffService shoppingService)
        {
            _shoppingBffService = shoppingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _shoppingBffService.GetCartAmount());
        }
    }
}
