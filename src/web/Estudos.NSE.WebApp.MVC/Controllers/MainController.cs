using Estudos.NSE.WebApp.MVC.Models;
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
    }
}