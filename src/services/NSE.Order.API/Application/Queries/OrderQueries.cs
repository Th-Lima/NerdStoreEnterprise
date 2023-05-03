using Dapper;
using NSE.Order.API.Application.Dto;
using NSE.Order.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Order.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDto> GetLastOrder(Guid clientId);
        Task<IEnumerable<OrderDto>> GetListByClientId(Guid clientId);
    }

    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository pedidoRepository)
        {
            _orderRepository = pedidoRepository;
        }

        public async Task<OrderDto> GetLastOrder(Guid clientId)
        {
            const string sql = @"SELECT
                                P.ID AS 'ProductId', P.CODE, P.VOUCHERUSED, P.DISCOUNT, P.TOTALVALUE, P.ORDERSTATUS,
                                P.ADDRESSPLACE, P.NUMBER, P.NEIGHBORHOOD, P.ZIPCODE, P.COMPLEMENT, P.CITY, P.STATE,
                                PIT.ID AS 'ProductItemId', PIT.PRODUCTNAME, PIT.AMOUNT, PIT.PRODUCTIMAGE, PIT.UNITVALUE 
                                FROM ORDERS P 
                                INNER JOIN ORDERITEMS PIT ON P.ID = PIT.ORDERID 
                                WHERE P.CLIENTID = @clientId 
                                AND P.CREATIONDATE between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.ORDERSTATUS = 1 
                                ORDER BY P.CREATIONDATE DESC";

            var order = await _orderRepository.GetConnect()
                .QueryAsync<dynamic>(sql, new { clientId });

            return MapearPedido(order);
        }

        public async Task<IEnumerable<OrderDto>> GetListByClientId(Guid clientId)
        {
            var order = await _orderRepository.GetListByClientId(clientId);

            return order.Select(OrderDto.ForOrderDto);
        }

        private OrderDto MapearPedido(dynamic result)
        {
            var order = new OrderDto
            {
                Code = result[0].CODE,
                Status = result[0].ORDERSTATUS,
                TotalValue = result[0].TOTALVALUE,
                Discount = result[0].DISCOUNT,
                VoucherUsed = result[0].VOUCHERUSED,

                OrderItems = new List<OrderItemDto>(),
                Address = new AddressDto
                {
                    AddressPlace = result[0].ADDRESSPLACE,
                    Neighborhood = result[0].NEIGHBORHOOD,
                    ZipCode = result[0].ZIPCODE,
                    City = result[0].CITY,
                    Complement = result[0].COMPLEMENT,
                    State = result[0].STATE,
                    NumberAddress = result[0].NUMBER
                }
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDto
                {
                    Name = item.PRODUCTNAME,
                    Price = item.UNITVALUE,
                    Amount = item.AMOUNT,
                    Image = item.PRODUCTIMAGE
                };

                order.OrderItems.Add(orderItem);
            }

            return order;
        }
    }
}
