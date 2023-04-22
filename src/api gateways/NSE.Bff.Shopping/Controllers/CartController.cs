using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Shopping.Services;
using NSE.WebAPI.Core.Controllers;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("shopping/cart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse();
        }

        [HttpGet]
        [Route("shopping/cart-amount")]
        public async Task<IActionResult> GetAmountCart()
        {
            return CustomResponse();
        }

        [HttpPost]
        [Route("shopping/cart/items")]
        public async Task<IActionResult> AddCartItem()
        {
            return CustomResponse();
        }

        [HttpPut]
        [Route("shopping/cart/items/{productId}")]
        public async Task<IActionResult> UpdateCartItem()
        {
            return CustomResponse();
        }

        [HttpDelete]
        [Route("shopping/cart/items/{productId}")]
        public async Task<IActionResult> RemoveCartItem()
        {
            return CustomResponse();
        }
    }
}
