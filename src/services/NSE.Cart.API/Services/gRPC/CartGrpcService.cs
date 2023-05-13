using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSE.Cart.API.Data;
using NSE.Cart.API.Models;
using NSE.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NSE.Cart.API.Services.gRPC
{
    [Authorize]
    public class CartGrpcService : CartShopping.CartShoppingBase
    {
        private readonly ILogger<CartGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly CartContext _context;

        public CartGrpcService(
            ILogger<CartGrpcService> logger,
            IAspNetUser user,
            CartContext context)
        {
            _logger = logger;
            _user = user;
            _context = context;
        }

        public override async Task<CartCustomerResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Chamando GetCart");

            var cart = await GetCartCustomer() ?? new CartCustomer();

            return MapCartCustomerToProtoResponse(cart);
        }

        private async Task<CartCustomer> GetCartCustomer()
        {
            return await _context.CartCustomers
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private static CartCustomerResponse MapCartCustomerToProtoResponse(CartCustomer cart)
        {
            var cartProto = new CartCustomerResponse
            {
                Id = cart.Id.ToString(),
                Customerid = cart.CustomerId.ToString(),
                Totalvalue = (double)cart.TotalValue,
                Discount = (double)cart.Discount,
                Voucherused = cart.VoucherUsed,
            };

            if (cart.Voucher != null)
            {
                cartProto.Voucher = new VoucherResponse
                {
                    Code = cart.Voucher.Code,
                    Percentage = (double?)cart.Voucher.Percentage ?? 0,
                    Valuediscount = (double?)cart.Voucher.ValueDiscount ?? 0,
                    Typediscount = (int)cart.Voucher.TypeDiscount
                };
            }

            foreach (var item in cart.Itens)
            {
                cartProto.Itens.Add(new CartItemResponse
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Productid = item.ProductId.ToString(),
                    Amount = item.Amount,
                    Price = (double)item.Price
                });
            }

            return cartProto;
        }
    }
}
