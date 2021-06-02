using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Core.Messages.Integrations;
using Estudos.NSE.MessageBus;
using MediatR;

namespace Estudos.NSE.Pedidos.API.Application.Events
{
    public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
    {
        private readonly IMessageBus _bus;

        public PedidoEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(PedidoRealizadoEvent notification, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new PedidoRealizadoIntegrationEvent(notification.ClienteId));
        }
    }
}