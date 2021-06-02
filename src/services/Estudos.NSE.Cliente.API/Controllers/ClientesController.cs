using System;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Application.Commands;
using Estudos.NSE.Clientes.API.Models;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.WebApi.Core.Controllers;
using Estudos.NSE.WebApi.Core.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Clientes.API.Controllers
{
    [Route("api/cliente")]
    public class ClientesController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;

        public ClientesController(IClienteRepository clienteRepository, IMediatorHandler mediator, IAspNetUser user)
        {
            _clienteRepository = clienteRepository;
            _mediator = mediator;
            _user = user;
        }

        [HttpGet("endereco")]
        public async Task<IActionResult> ObterEndereco()
        {
            var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());

            return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediator.EnviarComando(endereco));
        }
    }
}