using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Identity.API.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if(ValidOperation())
                return Ok(result);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(x => x.Errors);
            foreach (var error in errors)
            {
                AddErrorsProcessing(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !Erros.Any();
        }

        protected void AddErrorsProcessing(string error)
        {
            Erros.Add(error);
        }

        protected void ClearErrorsProcessing()
        {
            Erros.Clear();
        }
    }
}
