using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;
using NSE.Order.API.Application.Dto;
using NSE.Order.API.Application.Events;
using NSE.Order.Domain.Orders;
using NSE.Order.Domain.Vouchers;
using NSE.Order.Domain.Vouchers.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Order.API.Application.Commands
{
    public class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherRepository _voucherRepository;

        public OrderCommandHandler(IVoucherRepository voucherRepository,
                                    IOrderRepository pedidoRepository)
        {
            _voucherRepository = voucherRepository;
            _orderRepository = pedidoRepository;
        }

        public async Task<ValidationResult> Handle(AddOrderCommand message, CancellationToken cancellationToken)
        {
            // Validação do comando
            if (!message.IsValid()) 
                return message.ValidationResult;

            // Mapear Pedido
            var pedido = MappingOrder(message);

            // Aplicar voucher se houver
            if (!await ApplyVoucher(message, pedido)) 
                return ValidationResult;

            // Validar pedido
            if (!ValidateOrder(pedido)) 
                return ValidationResult;

            // Processar pagamento
            if (!ProcessPayment(pedido)) 
                return ValidationResult;

            // Se pagamento tudo ok!
            pedido.AuthorizeOrder();

            // Adicionar Evento
            pedido.AddEvent(new OrderRealizedEvent(pedido.Id, pedido.ClientId));

            // Adicionar Pedido Repositorio
            _orderRepository.Add(pedido);

            // Persistir dados de pedido e voucher
            return await PersistData(_orderRepository.UnitOfWork);
        }

        private Domain.Orders.Order MappingOrder(AddOrderCommand message)
        {
            var endereco = new Address
            {
                AddressPlace = message.Address.AddressPlace,
                Number = message.Address.NumberAddress,
                Complement = message.Address.Complement,
                Neighborhood = message.Address.Neighborhood,
                ZipCode = message.Address.ZipCode,
                City = message.Address.City,
                State = message.Address.State
            };

            var pedido = new Domain.Orders.Order(message.ClientId, message.TotalValue, message.OrderItems.Select(OrderItemDto.ForOrderItemDto).ToList(),
                message.VoucherUsed, message.Discount);

            pedido.AssignAddress(endereco);
            return pedido;
        }

        private async Task<bool> ApplyVoucher(AddOrderCommand message, Domain.Orders.Order order)
        {
            if (!message.VoucherUsed) 
                return true;

            var voucher = await _voucherRepository.GetVoucherByCode(message.VoucherCode);
            if (voucher == null)
            {
                AddError("O voucher informado não existe!");
                return false;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);
            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AddError(m.ErrorMessage));
                return false;
            }

            order.AssignVoucher(voucher);
            voucher.DebitAmount();

            _voucherRepository.Update(voucher);

            return true;
        }

        private bool ValidateOrder(Domain.Orders.Order order)
        {
            var orderOriginalValue = order.TotalValue;
            var orderDiscount = order.Discount;

            order.CalculateOrderValue();

            if (order.TotalValue != orderOriginalValue)
            {
                AddError("O valor total do pedido não confere com o cálculo do pedido");
                return false;
            }

            if (order.Discount != orderDiscount)
            {
                AddError("O valor total não confere com o cálculo do pedido");
                return false;
            }

            return true;
        }

        public bool ProcessPayment(Domain.Orders.Order order)
        {
            return true;
        }
    }
}
