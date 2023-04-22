using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Communication;
using System.Collections.Generic;
using System.Linq;

namespace NSE.WebAPI.Core.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
                return Ok(result);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Messages", Errors.ToArray() }
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

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddErrorsProcessing(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ResponseResult respnseResult)
        {
            ResponseHasErrors(respnseResult);

            return CustomResponse();
        }

        protected bool ResponseHasErrors(ResponseResult responseResult)
        {
            if (responseResult == null || responseResult.Errors.Messages.Any())
                return false;

            foreach (var message in responseResult.Errors.Messages)
            {
                AddErrorsProcessing(message);
            }

            return true;
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddErrorsProcessing(string error)
        {
            Errors.Add(error);
        }

        protected void ClearErrorsProcessing()
        {
            Errors.Clear();
        }

    }
}
