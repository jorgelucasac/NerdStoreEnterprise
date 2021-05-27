using System;
using System.Threading.Tasks;
using Estudos.NSE.Carrinho.API.Model;
using Estudos.NSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Carrinho.API.Controllers
{
    [Route("/api/carrinho")]
    [Authorize]
    public class CarrinhoController : MainController
    {
        [HttpGet]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem carrinhoItem)
        {
            return CustomResponse();
        }
        [HttpPut]
        public async Task<IActionResult> AtualizarItemCarrinho([FromRoute]Guid produtoId, CarrinhoItem item)
        {
            return CustomResponse();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverItemCarrinho([FromRoute]Guid produtoId)
        {
            return CustomResponse();
        }


    }
}