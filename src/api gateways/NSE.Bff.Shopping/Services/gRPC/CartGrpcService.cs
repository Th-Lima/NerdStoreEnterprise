using NSE.Bff.Shopping.Models;
using NSE.Cart.API.Services.gRPC;
using System;
using System.Threading.Tasks;

namespace NSE.Bff.Shopping.Services.gRPC
{
    public class CartGrpcService : ICartGrpcService
    {
        private readonly CartShopping.CartShoppingClient _cartShoppingClient;

        public CartGrpcService(CartShopping.CartShoppingClient cartShoppingClient)
        {
            _cartShoppingClient = cartShoppingClient;
        }

        public async Task<CartDto> GetCart()
        {
            var response = await _cartShoppingClient.GetCartAsync(new GetCartRequest());
            return MapCartCustomerProtoResponseToDto(response);
        }

        private static CartDto MapCartCustomerProtoResponseToDto(CartCustomerResponse cartResponse)
        {
            var cartDto = new CartDto
            {
                TotalValue = (decimal)cartResponse.Totalvalue,
                Discount = (decimal)cartResponse.Discount,
                VoucherUsed = cartResponse.Voucherused
            };

            if (cartResponse.Voucher != null)
            {
                cartDto.Voucher = new VoucherDto
                {
                    Code = cartResponse.Voucher.Code,
                    Percentage = (decimal?)cartResponse.Voucher.Percentage,
                    ValueDiscount = (decimal?)cartResponse.Voucher.Valuediscount,
                    DiscountType = cartResponse.Voucher.Typediscount
                };
            }

            foreach (var item in cartResponse.Itens)
            {
                cartDto.Itens.Add(new ItemCartDto
                {
                    Name = item.Name,
                    Image = item.Image,
                    ProductId = Guid.Parse(item.Productid),
                    Amount = item.Amount,
                    Price = (decimal)item.Price
                });
            }

            return cartDto;
        }
    }
}
