using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalog.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identity.Claims;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalog.API.Controllers
{
    //[Authorize]
    public class CatalogController : MainController
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //[AllowAnonymous]
        [HttpGet("catalog/products")]
        public async Task<PagedResult<Product>> Index([FromQuery] int pageSize = 8, [FromQuery] int page = 1, [FromQuery] string query = null)
        {
            return await _productRepository.GetAll(pageSize, page, query);
        }

        //[ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("catalog/products/{id}")]
        public async Task<Product> ProductDetails(Guid id)
        {
            return await _productRepository.GetById(id);
        }

        [HttpGet("catalog/products/list/{ids}")]
        public async Task<IEnumerable<Product>> ObterProdutosPorId(string ids)
        {
            return await _productRepository.GetProductsByIds(ids);
        }
    }
}
