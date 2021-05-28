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
        private readonly ICarrinhoService _carrinhoService;
        private readonly ICatalogoService _catalogoService;

        public CarrinhoController(ICarrinhoService carrinhoService, ICatalogoService catalogoService)
        {
            _carrinhoService = carrinhoService;
            _catalogoService = catalogoService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _carrinhoService.ObterCarrinho());
        }

        [HttpPost]
        [Route("adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

            ValidarItemCarrinho(produto, itemProduto.Quantidade);
            if (!OperacaoValida()) return View("Index", await _carrinhoService.ObterCarrinho());

            itemProduto.Nome = produto.Nome;
            itemProduto.Valor = produto.Valor;
            itemProduto.Imagem = produto.Imagem;

            var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);
            if (ResponsePossuiErros(resposta)) return View("Index", await _carrinhoService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            ValidarItemCarrinho(produto, quantidade);
            if (!OperacaoValida()) return View("Index", await _carrinhoService.ObterCarrinho());

            var itemProduto = new ItemProdutoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

            if (ResponsePossuiErros(resposta)) return View("Index", await _carrinhoService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            if (produto is null)
            {
                AdicionarErroValidacao("Produto inexistente");
                return View("Index", await _carrinhoService.ObterCarrinho());
            }


            var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(resposta)) return View("Index", await _carrinhoService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        private void ValidarItemCarrinho(ProdutoViewModel produto, int quantidade)
        {
            if (produto is null)
                AdicionarErroValidacao("Produto inexistente");
            else if (quantidade < 1)
                AdicionarErroValidacao($"Escolha ao menos uma unidade do produto: {produto.Nome}");
            else if (quantidade > produto.QuantidadeEstoque)
                AdicionarErroValidacao($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque");

        }
    }
}