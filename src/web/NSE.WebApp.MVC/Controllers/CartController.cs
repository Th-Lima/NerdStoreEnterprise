using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Cart;
using NSE.WebApp.MVC.Services.Catalog;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddCartItem(ItemProductViewModel itemProductViewModel)
        {
            var product = await _catalogService.GetById(itemProductViewModel.ProductId);

            ValidationCartItem(itemProductViewModel.Amount, product);

            if (!ValidOperation())
                return View("Index", await _cartService.GetCart());

            itemProductViewModel.Name = product.Name;
            itemProductViewModel.Price = product.Price;
            itemProductViewModel.Image = product.Image;

            var response = await _cartService.AddCartItem(itemProductViewModel);

            if (ResponseHasErrors(response))
                return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, int amount)
        {
            var product = await _catalogService.GetById(productId);

            ValidationCartItem(amount, product);

            if (!ValidOperation())
                return View("Index", await _cartService.GetCart());

            var itemProduct = new ItemProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                ProductId = productId,
                Amount = amount
            };

            var response = await _cartService.UpdateCartItem(productId, itemProduct);

            if (ResponseHasErrors(response))
                return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/remove-item")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

            if (product == null)
            {
                AddErrorValidation("Produto Inexistente");
                return View("Index", await _cartService.GetCart());
            }

            var response = await _cartService.RemoveCartItem(productId);

            if (ResponseHasErrors(response))
                return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        private void ValidationCartItem(int amount, ProductViewModel product)
        {
            if (product == null)
                AddErrorValidation("Produto Inexistente");

            if (amount < 1)
                AddErrorValidation($"Escolha ao menos uma unidade do produto {product.Name}");

            if (amount > product.StockAmount)
                AddErrorValidation($"O produto {product.Name} possui {product.StockAmount} unidades em estoque, você selecionou {amount}");
        }
    }
}
