using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Cart.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System;
using System.Threading.Tasks;

namespace NSE.Cart.API.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly IAspNetUser _user;

        public CartController(IAspNetUser user)
        {
            _user = user;
        }

        [HttpGet("cart")]
        public async Task<CartCustomer> GetCart()
        {
            return null;
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem cartItem)
        {
            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(CartItem cartItem)
        {
            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> DeleteCartItem(Guid cartItemId)
        {
            return CustomResponse();
        }
    }
}
