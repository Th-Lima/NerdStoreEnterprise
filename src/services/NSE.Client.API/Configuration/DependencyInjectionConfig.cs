using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Client.API.Application.Commands;
using NSE.Client.API.Application.Events;
using NSE.Client.API.Data;
using NSE.Client.API.Data.Repository;
using NSE.Client.API.Models;
using NSE.Client.API.Services;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.User;

namespace NSE.Client.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            
            //Command, CommandHandler
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<AddAddressComand, ValidationResult>, CustomerCommandHandler>();

            //Events / EventsHandler
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();

            //Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            //Context
            services.AddScoped<CustomersContext>();
        }
    }
}
