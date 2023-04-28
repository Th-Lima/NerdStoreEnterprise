using FluentValidation;
using NSE.Core.Messages;
using NSE.Order.API.Application.Dto;
using System;
using System.Collections.Generic;

namespace NSE.Order.API.Application.Commands
{
    public class AddOrderCommand : Command
    {
        // Pedido
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        // Voucher
        public string VoucherCode { get; set; }
        public bool VoucherUsed { get; set; }
        public decimal Discount { get; set; }

        // Endereco
        public AddressDto Address { get; set; }

        // Cartao
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardExpiration { get; set; }
        public string CvvCard { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AdicionarPedidoValidation : AbstractValidator<AddOrderCommand>
        {
            public AdicionarPedidoValidation()
            {
                RuleFor(c => c.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.OrderItems.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido precisa ter no mínimo 1 item");

                RuleFor(c => c.TotalValue)
                    .GreaterThan(0)
                    .WithMessage("Valor do pedido inválido");

                RuleFor(c => c.CardNumber)
                    .CreditCard()
                    .WithMessage("Número de cartão inválido");

                RuleFor(c => c.CardName)
                    .NotNull()
                    .WithMessage("Nome do portador do cartão requerido.");

                RuleFor(c => c.CvvCard.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("O CVV do cartão precisa ter 3 ou 4 números.");

                RuleFor(c => c.CardExpiration)
                    .NotNull()
                    .WithMessage("Data expiração do cartão requerida.");
            }
        }
    }
}
