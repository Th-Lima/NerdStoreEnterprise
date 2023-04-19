using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Cart.API.Data;
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
        private readonly CartContext _context;

        public CartController(IAspNetUser user, CartContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("cart")]
        public async Task<CartCustomer> GetCart()
        {
            return await GetCartCustomer() ?? new CartCustomer();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem cartItem)
        {
            var cart = await GetCartCustomer();

            if(cart == null)
                HandleNewCart(cartItem);
            else
                HandleCartAlreadyExists(cart, cartItem);

            if (!ValidOperation())
                return CustomResponse();

            var result = await _context.SaveChangesAsync();

            if(result <= 0)
                AddErrorsProcessing("Não foi possível persistir os dados no banco");

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

        private async Task<CartCustomer> GetCartCustomer()
        {
            return await _context.CartCustomers
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.CustomerId == _user.GetUserId());
        }

        private void HandleNewCart(CartItem item)
        {
            var cart = new CartCustomer(_user.GetUserId());
            cart.AddItem(item);

            _context.CartCustomers.Add(cart);
        }

        private void HandleCartAlreadyExists(CartCustomer cart, CartItem item)
        {
            var productItemExists = cart.CartItemAlreadyExists(item);

            cart.AddItem(item);

            if (productItemExists)
                _context.CartItems.Update(cart.GetByProductId(item.ProductId));
            else
                _context.CartItems.Add(item);

            _context.CartCustomers.Update(cart);
        }
    }
}
