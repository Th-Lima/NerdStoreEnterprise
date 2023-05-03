using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Cart;
using NSE.WebApp.MVC.Services.Customer;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class OrderController : MainController
    {
        private readonly ICustomerService _customerService;
        private readonly IShoppingBffService _shoppingBffService;

        public OrderController(ICustomerService clienteService,
            IShoppingBffService comprasBffService)
        {
            _customerService = clienteService;
            _shoppingBffService = comprasBffService;
        }

        [HttpGet]
        [Route("delivery-address")]
        public async Task<IActionResult> DeliveryAddress()
        {
            var cart = await _shoppingBffService.GetCart();
            if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

            var address = await _customerService.GetAddress();
            var pedido = _shoppingBffService.MapForOrder(cart, address);

            return View(pedido);
        }

        [HttpGet]
        [Route("payment")]
        public async Task<IActionResult> Payment()
        {
            var cart = await _shoppingBffService.GetCart();

            if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

            var order = _shoppingBffService.MapForOrder(cart, null);

            return View(order);
        }

        [HttpPost]
        [Route("finalize-order")]
        public async Task<IActionResult> FinalizeOrder(OrderTransactionViewModel orderTransaction)
        {
            if (!ModelState.IsValid) return View("Payment", _shoppingBffService.MapForOrder(
                await _shoppingBffService.GetCart(), null));

            var result = await _shoppingBffService.FinalizeOrder(orderTransaction);

            if (ResponseHasErrors(result))
            {
                var cart = await _shoppingBffService.GetCart();
                if (cart.Itens.Count == 0) return RedirectToAction("Index", "Cart");

                var orderMap = _shoppingBffService.MapForOrder(cart, null);
                return View("Payment", orderMap);
            }

            return RedirectToAction("OrderConcluded");
        }

        [HttpGet]
        [Route("order-concluded")]
        public async Task<IActionResult> OrderConcluded()
        {
            return View("ConfirmationOrder", await _shoppingBffService.GetLastOrder());
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _shoppingBffService.GetListByCustomerId());
        }
    }
}
