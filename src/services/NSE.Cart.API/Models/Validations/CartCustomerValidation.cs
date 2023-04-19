using FluentValidation;
using System;

namespace NSE.Cart.API.Models.Validations
{
    public class CartCustomerValidation : AbstractValidator<CartCustomer>
    {
        public CartCustomerValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Cliente não reconhecido");

            RuleFor(c => c.Itens.Count)
                .GreaterThan(0)
                .WithMessage("O carrinho não possui itens");

            RuleFor(c => c.TotalValue)
                .GreaterThan(0)
                .WithMessage("O valor total do carrinho precisa ser maior que 0");
        }
    }
}
