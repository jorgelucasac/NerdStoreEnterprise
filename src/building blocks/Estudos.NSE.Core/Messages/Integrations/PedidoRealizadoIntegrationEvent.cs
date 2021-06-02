using System;

namespace Estudos.NSE.Core.Messages.Integrations
{
    public class PedidoRealizadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }

        public PedidoRealizadoIntegrationEvent(Guid clienteId)
        {
            ClienteId = clienteId;
            AggregateId = clienteId;
        }
    }
}