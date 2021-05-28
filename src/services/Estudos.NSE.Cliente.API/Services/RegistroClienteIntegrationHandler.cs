using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Application.Commands;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Core.Messages.Integrations;
using Estudos.NSE.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estudos.NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus messageBus)
        {
            _serviceProvider = serviceProvider;
            _messageBus = messageBus;
        }

        /// <summary>
        /// realiza o subscribe na fila
        /// </summary>
        private void SetResponder()
        {
            _messageBus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(
                async request => await RegistrarCliente(request));
            _messageBus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void OnConnect(object? sender, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var sucesso = await mediator.EnviarComando(clienteCommand);

            return new ResponseMessage(sucesso);
        }
    }
}