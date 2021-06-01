using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Estudos.NSE.Pedidos.API.Application.Commands
{
    public class PedidoCommandHandler : CommandHandler
    ,IRequestHandler<AdicionarPedidoCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}