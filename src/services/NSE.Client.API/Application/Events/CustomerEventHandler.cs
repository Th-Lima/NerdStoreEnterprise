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

            return Task.CompletedTask;
        }
    }
}
