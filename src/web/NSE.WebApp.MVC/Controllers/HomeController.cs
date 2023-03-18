using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            switch (id)
            {
                case 500:
                    modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                    modelErro.Title = "Ocorreu um erro!";
                    modelErro.ErrorCode = id;
                    break;
                case 404:
                    modelErro.Message =
                    "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                    modelErro.Title = "Ops! Página não encontrada.";
                    modelErro.ErrorCode = id;
                    break;
                case 403:
                    modelErro.Message = "Você não tem permissão para fazer isto.";
                    modelErro.Title = "Acesso Negado";
                    modelErro.ErrorCode = id;
                    break;
                default:
                    return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
