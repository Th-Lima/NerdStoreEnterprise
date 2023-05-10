using Dapper;
using Microsoft.EntityFrameworkCore;
using NSE.Catalog.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalog.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> GetAll(int pageSize, int pageIndex, string query = null)
        {
            var sql = @$"SELECT * FROM Products 
                      WHERE (@Query IS NULL OR Name LIKE '%' + @Query + '%') 
                      ORDER BY [Name] 
                      OFFSET {pageSize * (pageIndex - 1)} ROWS 
                      FETCH NEXT {pageSize} ROWS ONLY 
                      SELECT COUNT(Id) FROM Products 
                      WHERE (@Query IS NULL OR Name LIKE '%' + @Query + '%')";

            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { Query = query });

            var products = multi.Read<Product>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Product>()
            {
                List = products,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };

            //Conceito Paginação - Entity Framework + LINQ 
            //return await _context.Product.AsNoTracking()
            //    .Skip(pageSize * (pageIndex - 1))
            //    .Take(pageSize)
            //    .Where(x => x.Name.Contains(query))
            //    .ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<List<Product>> GetProductsByIds(string ids)
        {
            var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Product>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Product.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();
        }

        public void Add(Product product)
        {
            _context.Product.Add(product);
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
