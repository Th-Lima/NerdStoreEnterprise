using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebAPI.Core.Controllers;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
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
