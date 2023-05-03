using NSE.Order.Domain.Orders;
using System;

namespace NSE.Order.API.Application.Dto
{
    public class OrderItemDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }

        public static OrderItem ForOrderItemDto(OrderItemDto orderItemDTO)
        {
            return new OrderItem(orderItemDTO.ProductId, orderItemDTO.Name, orderItemDTO.Amount,
                orderItemDTO.Price, orderItemDTO.Image);
        }
    }
}
