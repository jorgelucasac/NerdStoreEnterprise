using System;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;
using Estudos.NSE.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.WebApp.MVC.Controllers
{
    [Authorize, Route("carrinho")]
    public class CarrinhoController : MainController
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoController(IComprasBffService comprasBffService)
        {
            _comprasBffService = comprasBffService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _comprasBffService.ObterCarrinho());
        }

        [HttpPost]
        [Route("adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            var resposta = await _comprasBffService.AdicionarItemCarrinho(itemCarrinho);

            if (ResponsePossuiErros(resposta)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var resposta = await _comprasBffService.AtualizarItemCarrinho(produtoId, itemCarrinho);

            if (ResponsePossuiErros(resposta)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var resposta = await _comprasBffService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(resposta)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }
    }
}