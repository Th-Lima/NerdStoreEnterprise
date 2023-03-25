﻿using Microsoft.EntityFrameworkCore;
using NSE.Catalog.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Product.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Product.FindAsync(id);
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