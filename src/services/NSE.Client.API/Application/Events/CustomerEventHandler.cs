using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Client.API.Application.Events
{
    public class CustomerEventHandler : INotificationHandler<CustomerRegisteredEvent>
    {
        public Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
        {
            //Enviar evento de confirmação
            //TODO: Aqui por exemplo, poderia ser implementado um envio de email para o usuário cadastrado.

            return Task.CompletedTask;
        }
    }
}
