using FluentValidation;
using NSE.Core.Messages;
using System;

namespace NSE.Client.API.Application.Commands
{
    public class AddAddressComand : Command
    {
        public Guid CustomerId { get; set; }
        public string AddressPlace { get; set; }
        public string NumberAddress { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public AddAddressComand()
        {
        }

        public AddAddressComand(Guid customerId, string addressPlace, string numberAddress, string complement,
            string neighborhood, string zipcode, string city, string state)
        {
            AggregateId = customerId;
            CustomerId = customerId;
            AddressPlace = addressPlace;
            NumberAddress = numberAddress;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipcode;
            City = city;
            State = state;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddressValidation().Validate(this);

            return ValidationResult.IsValid;
        }

        public class AddressValidation : AbstractValidator<AddAddressComand>
        {
            public AddressValidation()
            {
                RuleFor(c => c.AddressPlace)
                    .NotEmpty()
                    .WithMessage("Informe o Logradouro");

                RuleFor(c => c.NumberAddress)
                    .NotEmpty()
                    .WithMessage("Informe o Número");

                RuleFor(c => c.ZipCode)
                    .NotEmpty()
                    .WithMessage("Informe o CEP");

                RuleFor(c => c.Neighborhood)
                    .NotEmpty()
                    .WithMessage("Informe o Bairro");

                RuleFor(c => c.City)
                    .NotEmpty()
                    .WithMessage("Informe o Cidade");

                RuleFor(c => c.State)
                    .NotEmpty()
                    .WithMessage("Informe o Estado");
            }
        }
    }
}
