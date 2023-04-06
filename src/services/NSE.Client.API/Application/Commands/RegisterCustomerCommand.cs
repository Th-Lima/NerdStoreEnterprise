using FluentValidation;
using NSE.Core.DomainObjects;
using NSE.Core.Messages;
using System;

namespace NSE.Client.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Cpf { get; private set; }

        public RegisterCustomerCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCustomerValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
    public class RegisterCustomerValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerValidation()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do cliente não foi informado");

            RuleFor(x => x.Cpf)
                .Must(HaveValidCPF)
                .WithMessage("O CPF informado não é válido");

            RuleFor(x => x.Email)
                .Must(HaveValidEmail)
                .WithMessage("O email informado não é válido");
        }

        protected static bool HaveValidCPF(string cpf)
        {
            return CPF.Validate(cpf);
        }

        protected static bool HaveValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
