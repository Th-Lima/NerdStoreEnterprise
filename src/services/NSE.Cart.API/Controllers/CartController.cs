using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Cart.API.Data;
using NSE.Cart.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System;
using System.Linq;
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

            await PersistData();

            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {
            var cart = await GetCartCustomer();
            var cartItem = await GetCartItemValidated(productId, cart, item);

            if(cartItem == null)
                return CustomResponse();

            cart.UpdateUnit(cartItem, item.Amount);

            ValidateCart(cart);
            if (!ValidOperation())
                return CustomResponse();

            _context.CartItems.Update(cartItem);
            _context.CartCustomers.Update(cart);

            await PersistData();

            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> DeleteCartItem(Guid productId)
        {
            var cart = await GetCartCustomer();

            var cartItem = await GetCartItemValidated(productId, cart);

            if (cartItem == null)
                return CustomResponse();

            ValidateCart(cart);
            if (!ValidOperation())
                return CustomResponse();

            cart.RemoveItem(cartItem);

            _context.CartItems.Remove(cartItem);
            _context.CartCustomers.Update(cart);

            await PersistData();

            return CustomResponse();
        }

        [HttpPost]
        [Route("cart/apply-voucher")]
        public async Task<IActionResult> AplicarVoucher(Voucher voucher)
        {
            var carrinho = await GetCartCustomer();

            carrinho.ApplyVoucher(voucher);

            _context.CartCustomers.Update(carrinho);

            await PersistData();

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

            ValidateCart(cart);

            _context.CartCustomers.Add(cart);
        }

        private void HandleCartAlreadyExists(CartCustomer cart, CartItem item)
        {
            var productItemExists = cart.CartItemAlreadyExists(item);

            cart.AddItem(item);

            ValidateCart(cart);

            if (productItemExists)
                _context.CartItems.Update(cart.GetByProductId(item.ProductId));
            else
                _context.CartItems.Add(item);
            
            _context.CartCustomers.Update(cart);
        }

        private async Task<CartItem> GetCartItemValidated(Guid productId, CartCustomer cart, CartItem item = null)
        {
            if(item != null && productId != item.ProductId)
            {
                AddErrorsProcessing("O item não corresponde ao informado");
                return null;
            }

            if(cart == null)
            {
                AddErrorsProcessing("Carrinho não encontrado");
                return null;
            }

            var itemCart = await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == productId);

            if(itemCart == null || !cart.CartItemAlreadyExists(itemCart))
            {
                AddErrorsProcessing("O item não está no carrinho");
                return null;
            }

            return itemCart;
        }

        private async Task PersistData()
        {
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
                AddErrorsProcessing("Não foi possível persistir os dados no banco");
        }

        private bool ValidateCart(CartCustomer cart)
        {
            if (cart.IsValid())
                return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddErrorsProcessing(e.ErrorMessage));

            return false;
        }
    }
}
