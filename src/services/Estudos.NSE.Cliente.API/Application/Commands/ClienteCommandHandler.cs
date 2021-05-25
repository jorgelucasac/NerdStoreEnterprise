using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Models;
using FluentValidation.Results;

namespace Estudos.NSE.Clientes.API.Application.Commands
{
    public class ClienteCommandHandler : IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            //validações de negócio

            //persistir no banco


            return null;
        }
    }
}