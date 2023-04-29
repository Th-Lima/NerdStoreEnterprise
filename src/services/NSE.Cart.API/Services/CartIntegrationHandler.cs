using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Cart.API.Data;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cart.API.Services
{
    public class CartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CartIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        //Subscribe - Escuta fila
        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderRealizedIntegrationEvent>("OrderRealized", async request =>
                await DeleteCart(request));
        }

        private async Task DeleteCart(OrderRealizedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartContext>();

            var carrinho = await context.CartCustomers
                .FirstOrDefaultAsync(c => c.CustomerId == message.ClientId);

            if (carrinho != null)
            {
                context.CartCustomers.Remove(carrinho);
                await context.SaveChangesAsync();
            }
        }
    }
}
