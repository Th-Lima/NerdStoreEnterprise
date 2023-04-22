using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NSE.Core.Communication;

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

        protected void AddErrorValidation(string message)
        {
            ModelState.AddModelError(string.Empty, message);
        }

        protected bool ValidOperation()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
