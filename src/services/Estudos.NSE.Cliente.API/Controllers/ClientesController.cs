using System;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Application.Commands;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Clientes.API.Controllers
{
    [Route("api/[controller]")]
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("obter-todos")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _mediatorHandler.EnviarComando(
                new RegistrarClienteCommand(Guid.NewGuid(), "jorge", "jorge@teste.com", "30314299076"));

            return CustomResponse(resultado);
        }
    }
}