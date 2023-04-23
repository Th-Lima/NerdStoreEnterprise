using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Order.API.Controllers
{
    [Authorize]
    public class VoucherController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
