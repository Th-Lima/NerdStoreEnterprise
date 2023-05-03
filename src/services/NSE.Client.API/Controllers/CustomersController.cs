using Microsoft.AspNetCore.Mvc;
using NSE.Client.API.Application.Commands;
using NSE.Client.API.Models;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NSE.Client.API.Controllers
{
    public class CustomersController : MainController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;

        public CustomersController(ICustomerRepository clienteRepository, IMediatorHandler mediator, IAspNetUser user)
        {
            _customerRepository = clienteRepository;
            _mediator = mediator;
            _user = user;
        }

        [HttpGet("customer/address")]
        public async Task<IActionResult> GetAddress()
        {
            var address = await _customerRepository.GetAddressById(_user.GetUserId());

            return address == null ? NotFound() : CustomResponse(address);
        }

        [HttpPost("customer/address")]
        public async Task<IActionResult> AddAddress(AddAddressComand address)
        {
            address.CustomerId = _user.GetUserId();

            return CustomResponse(await _mediator.SendCommand(address));
        }
    }
}
