using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Extensions
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList modeloPaginado)
        {
            return View(modeloPaginado);
        }
    }

   
}