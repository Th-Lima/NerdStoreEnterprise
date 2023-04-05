using FluentValidation.Results;
using MediatR;
using NSE.Client.API.Models;
using NSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Client.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
                return  message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.Cpf);

            //Validação negocio

            //Persistir no banco

            if (true) //Já existe um cliente com o cpf informado
            {
                AddError("Este CPF já está em uso");
                return ValidationResult;
            }

            return message.ValidationResult;
        }
    }
}
