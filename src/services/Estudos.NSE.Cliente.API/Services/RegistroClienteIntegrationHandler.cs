using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Estudos.NSE.Clientes.API.Application.Commands;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Core.Messages.Integrations;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estudos.NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(
                async request => new ResponseMessage(await RegistrarCliente(request)));


            //var resultado = await _mediatorHandler.EnviarComando(
            //    
            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var sucesso = await mediator.EnviarComando(clienteCommand);

            return sucesso;
        }
    }
}