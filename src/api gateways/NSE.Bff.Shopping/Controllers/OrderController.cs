using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Shopping.Models;
using NSE.Bff.Shopping.Services;
using NSE.WebAPI.Core.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Controllers
{
    public class OrderController : MainController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(
            ICatalogService catalogService,
            ICartService cartService,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost]
        [Route("shopping/order")]
        public async Task<IActionResult> AddOrder(OrderDto order)
        {
            var cart = await _cartService.GetCart();
            var products = await _catalogService.GetItems(cart.Itens.Select(p => p.ProductId));
            var address = await _customerService.GetAddress();

            if (!await ValidateCartProducts(cart, products)) 
                return CustomResponse();

            AssignDataOrder(cart, address, order);

            return CustomResponse(await _orderService.FinalizeOrder(order));
        }

        [HttpGet("shopping/order/last")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderService.GetLastOrder();
            if (order is null)
            {
                AddErrorsProcessing("Pedido não encontrado!");

                return CustomResponse();
            }

            return CustomResponse(order);
        }

        [HttpGet("shopping/order/list-customer")]
        public async Task<IActionResult> ListByCustomer()
        {
            var orders = await _orderService.GetListByCustomerId();

            return orders == null ? NotFound() : CustomResponse(orders);
        }

        private async Task<bool> ValidateCartProducts(CartDto cart, IEnumerable<ItemProductDto> products)
        {
            if (cart.Itens.Count != products.Count())
            {
                var itensUnavailable = cart.Itens.Select(c => c.ProductId).Except(products.Select(p => p.Id)).ToList();

                foreach (var itemId in itensUnavailable)
                {
                    var itemCarrinho = cart.Itens.FirstOrDefault(c => c.ProductId == itemId);

                    AddErrorsProcessing($"O item {itemCarrinho.Name} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var itemCart in cart.Itens)
            {
                var productCatalog = products.FirstOrDefault(p => p.Id == itemCart.ProductId);

                if (productCatalog.Price != itemCart.Price)
                {
                    var msgErro = $"O produto {itemCart.Name} mudou de valor (de: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCart.Price)} para: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", productCatalog.Price)}) desde que foi adicionado ao carrinho.";

                    AddErrorsProcessing(msgErro);

                    var responseRemove = await _cartService.RemoveItemCart(itemCart.ProductId);
                    if (ResponseHasErrors(responseRemove))
                    {
                        AddErrorsProcessing($"Não foi possível remover automaticamente o produto {itemCart.Name} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    itemCart.Price = productCatalog.Price;
                    var responseAdd = await _cartService.AddItemCart(itemCart);

                    if (ResponseHasErrors(responseAdd))
                    {
                        AddErrorsProcessing($"Não foi possível atualizar automaticamente o produto {itemCart.Name} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    ClearErrorsProcessing();
                    AddErrorsProcessing(msgErro + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void AssignDataOrder(CartDto cart, AddressDto address, OrderDto order)
        {
            order.VoucherCode = cart.Voucher?.Code;
            order.VoucherUsed = cart.VoucherUsed;
            order.TotalValue = cart.TotalValue;
            order.Discount = cart.Discount;
            order.OrderItems = cart.Itens;

            order.Address = address;
        }
    }
}
