using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    public class PedidoController : MainController
    {
        private readonly IComprasBffService _comprasBffService;
        private readonly IClienteService _clienteService;

        public PedidoController(IComprasBffService comprasBffService, IClienteService clienteService)
        {
            _comprasBffService = comprasBffService;
            _clienteService = clienteService;
        }

        [HttpGet]
        [Route("endereco-de-entrega")]
        public async Task<IActionResult> EnderecoEntrega()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            if (carrinho.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var endereco = await _clienteService.ObterEndereco();
            var pedido = _comprasBffService.MapearParaPedido(carrinho, endereco);

            return View(pedido);
        }
    }
}