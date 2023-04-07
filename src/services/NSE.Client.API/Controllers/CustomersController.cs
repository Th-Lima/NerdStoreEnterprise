using Microsoft.AspNetCore.Mvc;
using NSE.Client.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Threading.Tasks;

namespace NSE.Client.API.Controllers
{
    public class CustomersController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomersController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler.SendCommand(new RegisterCustomerCommand(Guid.NewGuid(), "Thales", "thales@teste.com", "14048470060"));

            return CustomResponse(result);
        }
    }
}
