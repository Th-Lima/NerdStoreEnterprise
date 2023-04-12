using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Messages.Integration;
using NSE.Identity.API.Models;
using NSE.Identity.API.Services.Interfaces;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Threading.Tasks;

namespace NSE.Identity.API.Controllers
{
    [Route("api/identity")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        private IBus _bus;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService, IBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _bus = bus;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register(UserIdentityRegister userRegister)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded)
            {
                //Evento Integração
                var success = await RegisterCustomer(userRegister);
                                
                return CustomResponse(await _jwtService.GenerateJwtAsync(userRegister.Email));
            }

            foreach (var resultError in result.Errors)
            {
                AddErrorsProcessing(resultError.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterCustomer(UserIdentityRegister userRegister)
        {
            var user = await _userManager.FindByNameAsync(userRegister.Email);
            var userRegistered = new UserRegisteredIntegrationEvent(Guid.Parse(user.Id), userRegister.Name, userRegister.Email, userRegister.Cpf);

            _bus = RabbitHutch.CreateBus("host=localhost:5672");

            var success = await _bus.Rpc.RequestAsync<UserRegisteredIntegrationEvent, ResponseMessage>(userRegistered);

            return success;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserIdentityLogin userLogin)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
                return CustomResponse(await _jwtService.GenerateJwtAsync(userLogin.Email));

            if (result.IsLockedOut)
            {
                AddErrorsProcessing("Usuário temporariamente bloqueado por tentativas inválidas");

                return CustomResponse();
            }

            AddErrorsProcessing("Usuário ou Senha incorretos");

            return CustomResponse();
        }
    }
}
