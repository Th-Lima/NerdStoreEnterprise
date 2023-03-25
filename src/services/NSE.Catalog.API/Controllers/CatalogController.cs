using Microsoft.AspNetCore.Mvc;
using NSE.Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalog.API.Controllers
{
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.GetAll();
        }

        [HttpGet("catalog/products/{id}")]
        public async Task<Product> ProductDetails(Guid id)
        {
            return await _productRepository.GetById(id);
        }
    }
}
