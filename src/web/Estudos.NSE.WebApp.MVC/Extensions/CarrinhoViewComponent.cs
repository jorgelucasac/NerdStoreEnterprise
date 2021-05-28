using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;
using Estudos.NSE.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly ICarrinhoService _carrinhoService;

        public CarrinhoViewComponent(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _carrinhoService.ObterQuantidadeItensCarrinho());
        }
    }
}