using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSE.WebApp.MVC.Services.Catalog;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogController : MainController
    {
        public readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("showcase")]
        public async Task<IActionResult> Index([FromQuery] int pageSize = 8, [FromQuery] int pageIndex = 1, [FromQuery] string query = null)
        {
            var products = await _catalogService.GetAll(pageSize, pageIndex, query);

            ViewBag.Search = query;
            products.ReferenceAction = "Index";

            return View(products);
        }

        [HttpGet]
        [Route(("product-detail/{id}"))]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _catalogService.GetById(id);

            return View(product);
        }
    }
}
