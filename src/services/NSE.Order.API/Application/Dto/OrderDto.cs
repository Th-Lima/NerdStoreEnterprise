using System;
using System.Collections.Generic;

namespace NSE.Order.API.Application.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }

        public static OrderDto ForOrderDto(Domain.Orders.Order order)
        {
            var orderDto = new OrderDto
            {
                Id = order.Id,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                Date = order.CreationDate,
                TotalValue = order.TotalValue,
                Discount = order.Discount,
                VoucherUsed = order.VoucherUsed,
                OrderItems = new List<OrderItemDto>(),
                Address = new AddressDto()
            };

            foreach (var item in order.OrderItems)
            {
                orderDto.OrderItems.Add(new OrderItemDto
                {
                    Name = item.ProductName,
                    Image = item.ProductImage,
                    Amount = item.Amount,
                    ProductId = item.ProductId,
                    Value = item.UnitValue,
                    OrderId = item.OrderId
                });
            }

            orderDto.Address = new AddressDto
            {
                AddressPlace = order.Address.AddressPlace,
                NumberAddress = order.Address.Number,
                Complement = order.Address.Complement,
                Neighborhood = order.Address.Neighborhood,
                ZipCode = order.Address.ZipCode,
                City = order.Address.City,
                State = order.Address.State,
            };

            return orderDto;
        }
    }
}
