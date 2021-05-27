using System;
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
            return await _carrinhoRepository.ObterCarrionhoCliente(_user.ObterUserId()) ?? new CarrinhoCliente();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem carrinhoItem)
        {

            var carrinho = await _carrinhoRepository.ObterCarrionhoCliente(_user.ObterUserId());
            if (carrinho is null)
                await ManipularNovoCarinho(carrinhoItem);
            else
                await ManipularCarinhoExistente(carrinho, carrinhoItem);

            if (!OperacaoValida()) return CustomResponse();

            var sucesso = await _carrinhoRepository.SaveChangesAsync();

            if (!sucesso)
                AdicionarErroProcessamento("Não foi possível persistir os dados no banco");

            return CustomResponse();
        }
        [HttpPut]
        public async Task<IActionResult> AtualizarItemCarrinho([FromRoute] Guid produtoId, CarrinhoItem item)
        {
            return CustomResponse();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverItemCarrinho([FromRoute] Guid produtoId)
        {
            return CustomResponse();
        }

        #region privates

        private async Task ManipularNovoCarinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_user.ObterUserId());
            carrinho.AdicionarItem(item);
            await _carrinhoRepository.Adicionar(carrinho);
        }

        private async Task ManipularCarinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var carrinhoItemExiste = carrinho.CarrinhoItemExiste(item);
            carrinho.AdicionarItem(item);

            if (carrinhoItemExiste)
                _carrinhoRepository.AtualizarItem(carrinho.ObterProdutoPorId(item.Id));
            else
                await _carrinhoRepository.AdicionarItem(item);


            await _carrinhoRepository.Adicionar(carrinho);
        }

        #endregion

    }
}