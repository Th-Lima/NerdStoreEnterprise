using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Client.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Client.API.Services
{
    public class RecordCustomerIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RecordCustomerIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();

            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegisteredIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult success;

            //Service Locator =>
            //In this scenario this practice is required, because MediatorHandler is injected with life cycle >>Scoped<< and this class "RecordCustomerIntegrationHandler" is injected with lyfe cycle >>Singleton<<
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                success = await mediator.SendCommand(customerCommand);
            }

            return new ResponseMessage(success);
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
               await RegisterCustomer(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }
    }
}
