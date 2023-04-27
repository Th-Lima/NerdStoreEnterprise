using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Order.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Order.Infra.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => _context;

        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public DbConnection GetConnect()
        {
            return _context.Database.GetDbConnection();
        }


        public async Task<Domain.Orders.Order> GetById(Guid id)
        {
            return await _context.Order.FindAsync(id);
        }

        public async Task<OrderItem> GetItemById(Guid id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);
        }

        public async Task<IEnumerable<Domain.Orders.Order>> GetListByClientId(Guid clientId)
        {
            return await _context.Order
               .Include(p => p.OrderItems)
               .AsNoTracking()
               .Where(p => p.ClientId == clientId)
               .ToListAsync();
        }

        public void Add(Domain.Orders.Order order)
        {
            _context.Order.Add(order);
        }

        public void Update(Domain.Orders.Order order)
        {
            _context.Order.Update(order);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
