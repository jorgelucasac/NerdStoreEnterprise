using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Carrinho.API.Model;
using Estudos.NSE.Core.Messages.Integrations;
using Estudos.NSE.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estudos.NSE.Carrinho.API.Services
{
    public class CarrinhoIntegrationHandler: BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public CarrinhoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado",
            async request=> await ApagarCarrinho(request));
            _messageBus.AdvancedBus.Connected += OnConnect;

        }
        private void OnConnect(object? sender, EventArgs e)
        {
            SetSubscribers();
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var carrinhoRepository = scope.ServiceProvider.GetRequiredService<ICarrinhoRepository>();

            var carrinho = await carrinhoRepository.ObterCarrinhoCliente(message.ClienteId);

            if (carrinho != null)
            {
                carrinhoRepository.RemoverItens(carrinho.Itens);
                carrinhoRepository.Remover(carrinho);
                await carrinhoRepository.SaveChangesAsync();
            }
        }
    }
}