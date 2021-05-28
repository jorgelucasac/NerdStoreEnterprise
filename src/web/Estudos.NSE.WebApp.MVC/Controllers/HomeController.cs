using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.AspNetCore.Http;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      
        [Route("privacidade")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
                Titulo = "Sistema indisponível.",
                ErroCode = StatusCodes.Status500InternalServerError
            };

            return View("Error", modelErro);
        }

        [Route("erro/{id}")]
        public IActionResult Error(int id)
        {
            var erro = new ErrorViewModel();
            switch (id)
            {
                case StatusCodes.Status404NotFound:
                    erro.Mensagem = "A página que você está procurando não existe. <br/>" +
                                    "Em caso de dúvidas entre em contato com o suporte.";
                    erro.Titulo = "Ops! Página não encontrada";
                    erro.ErroCode = id;
                    break;
                case StatusCodes.Status403Forbidden:
                    erro.Mensagem = "Você não tem permissão para fazer isso!";
                    erro.Titulo = "Acesso Negado";
                    erro.ErroCode = id;
                    break;

                case StatusCodes.Status500InternalServerError:
                    erro.ErroCode = StatusCodes.Status500InternalServerError;
                    erro.Titulo = "Ops! Erro Interno do Servidor!";
                    erro.Mensagem = "Infelizmente, estamos com problemas para carregar a página que você está procurando. Volte daqui a pouco.";
                    break;

                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(erro);
        }
    }
}
