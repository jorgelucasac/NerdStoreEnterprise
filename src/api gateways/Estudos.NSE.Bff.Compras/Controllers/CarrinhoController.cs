using System;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Models;
using Estudos.NSE.Bff.Compras.Services;
using Estudos.NSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Bff.Compras.Controllers
{
    [Authorize]
    [Route("api/compras")]
    public class CarrinhoController : MainController
    {
        private readonly ICarrinhoService _carrinhoService;
        private readonly ICatalogoService _catalogoService;

        public CarrinhoController(
            ICarrinhoService carrinhoService,
            ICatalogoService catalogoService)
        {
            _carrinhoService = carrinhoService;
            _catalogoService = catalogoService;
        }

        [HttpGet]
        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _carrinhoService.ObterCarrinho());
        }

        [HttpGet]
        [Route("carrinho-quantidade")]
        public async Task<IActionResult> ObterQuantidadeCarrinho()
        {
            return CustomResponse(await _carrinhoService.ObterQuantidadeCarrinho());
        }

        [HttpPost]
        [Route("carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDto itemCarrinhoDto)
        {
            var produto = await _catalogoService.ObterPorId(itemCarrinhoDto.ProdutoId);

            await ValidarItemCarrinho(produto, itemCarrinhoDto.Quantidade);
            if (!OperacaoValida()) return CustomResponse();

            itemCarrinhoDto.Nome = produto.Nome;
            itemCarrinhoDto.Valor = produto.Valor;
            itemCarrinhoDto.Imagem = produto.Imagem;

            var resposta = await _carrinhoService.AdicionarItemCarrinho(itemCarrinhoDto);
            return CustomResponse(resposta);
        }

        [HttpPut]
        [Route("carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto itemCarrinhoDto)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            await ValidarItemCarrinho(produto, itemCarrinhoDto.Quantidade);
            if (!OperacaoValida()) return CustomResponse();

            var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemCarrinhoDto);
            return CustomResponse(resposta);
        }

        [HttpDelete]
        [Route("carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            if (produto == null)
            {
                AdicionarErroProcessamento("Produto Inexistente");
                return CustomResponse();
            }

            var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);
            return CustomResponse(resposta);
        }

        #region privados

        private async Task ValidarItemCarrinho(ItemProdutoDto produto, int quantidade)
        {
            if (produto is null)
            {
                AdicionarErroProcessamento("Produto inexistente");
                return;
            }
            if (quantidade < 1)
            {
                AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto: {produto.Nome}");
                return;
            }

            var carrinho = await _carrinhoService.ObterCarrinho();
            var itemCarrinho = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produto.Id);

            if (itemCarrinho != null && itemCarrinho.Quantidade + quantidade > produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
                return;
            }
            if (quantidade > produto.QuantidadeEstoque)
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
        }

        #endregion
    }
}
