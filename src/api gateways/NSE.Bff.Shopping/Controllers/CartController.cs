using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Shopping.Models;
using NSE.Bff.Shopping.Services;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Linq;
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
            return CustomResponse(_cartService.GetCart());
        }

        [HttpGet]
        [Route("shopping/cart-amount")]
        public async Task<int> GetAmountCart()
        {
            var amount = await _cartService.GetCart();

            return amount?.Itens.Sum(x => x.Amount) ?? 0;
        }

        [HttpPost]
        [Route("shopping/cart/items")]
        public async Task<IActionResult> AddCartItem(ItemCartDto itemProduct)
        {
            var product = await _catalogService.GetById(itemProduct.ProductId);

            await ValidateItemCart(product, itemProduct.Amount);
            
            if (!ValidOperation())
                return CustomResponse();

            itemProduct.Name = product.Name;
            itemProduct.Price = product.Price;
            itemProduct.Image = product.Image;

            var response = _cartService.AddItemCart(itemProduct);

            return CustomResponse(response.Result);
        }

        [HttpPut]
        [Route("shopping/cart/items/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, ItemCartDto itemProduct)
        {
            var produto = await _catalogService.GetById(productId);

            await ValidateItemCart(produto, itemProduct.Amount);

            if (!ValidOperation())
                return CustomResponse();

            var resposta = await _cartService.UpdateItemCart(productId, itemProduct);

            return CustomResponse(resposta);
        }

        [HttpDelete]
        [Route("shopping/cart/items/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

            if (product == null)
            {
                AddErrorsProcessing("Produto inexistente!");
                return CustomResponse();
            }

            var response = await _cartService.RemoveItemCart(productId);

            return CustomResponse(response);
        }

        private async Task ValidateItemCart(ItemProductDto product, int amount, bool addProduct = false)
        {
            if (product == null) 
                AddErrorsProcessing("Produto inexistente!");

            if (amount < 1) 
                AddErrorsProcessing($"Escolha ao menos uma unidade do produto {product.Name}");

            var carrinho = await _cartService.GetCart();
            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProductId == product.Id);

            var errorStockAmount = $"O produto {product.Name} possui {product.StockAmount} unidades em estoque, você selecionou {amount}";

            if (itemCarrinho != null && addProduct && itemCarrinho.Amount + amount > product.StockAmount)
            {
                AddErrorsProcessing(errorStockAmount);
                return;
            }

            if (amount > product.StockAmount) 
                AddErrorsProcessing(errorStockAmount);
        }
    }
}
