using System.Linq;
using Estudos.NSE.Core.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult result)
        {
            if (result == null || !result.Errors.Mensagens.Any()) return false;

            foreach (var mensagem in result.Errors.Mensagens)
            {
                ModelState.AddModelError(string.Empty, mensagem);
            }

            return true;

        }

        protected void AdicionarErroValidacao(string mensagemErro)
        {
            ModelState.AddModelError(string.Empty, mensagemErro);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}