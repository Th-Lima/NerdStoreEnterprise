using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Client.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Client.API.Services
{
    public class RecordCustomerIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RecordCustomerIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");

            _bus.Rpc.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
            new ResponseMessage(await RegisterCustomer(request)));

            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegisterCustomer(UserRegisteredIntegrationEvent message)
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

            return success;
        }
    }
}
