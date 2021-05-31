using Estudos.NSE.Core.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult result)
        {
            if (result == null) return false;

            foreach (var mensagen in result.Errors.Mensagens)
            {
                ModelState.AddModelError(string.Empty, mensagen);
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