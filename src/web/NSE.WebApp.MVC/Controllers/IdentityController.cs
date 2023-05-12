using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Identity;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentityController : MainController
    {
        private readonly IAuthService _authService;

        public IdentityController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("new-account")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("new-account")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if(!ModelState.IsValid)
                return View(userRegister);

            //HTTP request to Identity API (NSE.Identiy.API)
            var response = await _authService.RegisterAsync(userRegister);

            if(ResponseHasErrors(response.ResponseResult))
                return View(userRegister);

            await _authService.PerformLogin(response);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(userLogin);

            //HTTP request to Identity API (NSE.Identiy.API)
            var response = await _authService.LoginAsync(userLogin);

            if (ResponseHasErrors(response.ResponseResult))
                return View(userLogin);

            await _authService.PerformLogin(response);

            if(string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}
