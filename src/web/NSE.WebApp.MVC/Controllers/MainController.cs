using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (var errorMessage in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                return true;
            }

            return false;
        }
    }
}
