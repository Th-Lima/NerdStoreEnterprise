using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSE.Client.API.Application.Commands;
using NSE.Client.API.Application.Events;
using NSE.Client.API.Data;
using NSE.Client.API.Data.Repository;
using NSE.Client.API.Models;
using NSE.Client.API.Services;
using NSE.Core.Mediator;

namespace NSE.Client.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            
            //Command, CommandHandler and Event, EventHandler
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();

            //Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            //Context
            services.AddScoped<CustomersContext>();
            

            //IntegrationHandler
            services.AddHostedService<RecordCustomerIntegrationHandler>();
        }
    }
}
