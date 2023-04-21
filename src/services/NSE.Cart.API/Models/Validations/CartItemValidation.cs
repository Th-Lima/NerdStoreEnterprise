using FluentValidation;
using System;

namespace NSE.Cart.API.Models.Validations
{
    public class CartItemValidation : AbstractValidator<CartItem>
    {
        public CartItemValidation()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome do produto não foi informado");

            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage(item => $"A quantidade miníma para o {item.Name} é 1");

            RuleFor(c => c.Amount)
                .LessThanOrEqualTo(CartCustomer.MAX_AMOUNT_ITEM)
                .WithMessage(item => $"A quantidade máxima do {item.Name} é {CartCustomer.MAX_AMOUNT_ITEM}");

            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage(item => $"O valor do {item.Name} precisa ser maior que 0");
        }
    }
}
