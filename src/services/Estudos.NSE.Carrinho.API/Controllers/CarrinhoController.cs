using System;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Carrinho.API.Model;
using Estudos.NSE.WebApi.Core.Controllers;
using Estudos.NSE.WebApi.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Carrinho.API.Controllers
{
    [Route("/api/carrinho")]
    [Authorize]
    public class CarrinhoController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly ICarrinhoRepository _carrinhoRepository;

        public CarrinhoController(IAspNetUser user, ICarrinhoRepository carrinhoRepository)
        {
            _user = user;
            _carrinhoRepository = carrinhoRepository;
        }

        [HttpGet]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoCliente(_user.ObterUserId()) ?? new CarrinhoCliente(_user.ObterUserId());
            return carrinho;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem carrinhoItem)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoCliente(_user.ObterUserId());
            if (carrinho is null)
                await ManipularNovoCarinho(carrinhoItem);
            else
                await ManipularCarinhoExistente(carrinho, carrinhoItem);


            if (!OperacaoValida()) return CustomResponse();

            await PersistirDados();

            return CustomResponse();
        }

        [HttpPut("{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoCliente(_user.ObterUserId());
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
            if (itemCarrinho is null) return CustomResponse();
            carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            _carrinhoRepository.AtualizarItem(itemCarrinho);
            _carrinhoRepository.Atualizar(carrinho);
            await PersistirDados();

            return CustomResponse();
        }


        [HttpDelete("{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await _carrinhoRepository.ObterCarrinhoCliente(_user.ObterUserId());
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
            if (itemCarrinho is null) return CustomResponse();
            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _carrinhoRepository.RemoverItem(itemCarrinho);
            _carrinhoRepository.Atualizar(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        #region privates

        private async Task ManipularNovoCarinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_user.ObterUserId());
            carrinho.AdicionarItem(item);

            ValidarCarrinho(carrinho);
            await _carrinhoRepository.Adicionar(carrinho);
        }

        private async Task ManipularCarinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var carrinhoItemExiste = carrinho.CarrinhoItemExiste(item);
            carrinho.AdicionarItem(item);

            if (carrinhoItemExiste)
                _carrinhoRepository.AtualizarItem(carrinho.ObterProdutoPorId(item.ProdutoId));
            else
                await _carrinhoRepository.AdicionarItem(item);

            ValidarCarrinho(carrinho);
            _carrinhoRepository.Atualizar(carrinho);
        }

        private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
        {
            if (item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item não corresponde ao id informado!");
                return null;
            }

            if (carrinho is null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado!");
                return null;
            }

            var itemCarrinho = await _carrinhoRepository.ObterCarrinhoItem(carrinho.Id, produtoId);

            if (itemCarrinho is null)
                AdicionarErroProcessamento("O item não está no carrinho!");

            return itemCarrinho;

        }

        private async Task PersistirDados()
        {
            var sucesso = await _carrinhoRepository.SaveChangesAsync();
            if (!sucesso)
                AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
        }

        private bool ValidarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(i => AdicionarErroProcessamento(i.ErrorMessage));
            return false;
        }
        #endregion

    }
}