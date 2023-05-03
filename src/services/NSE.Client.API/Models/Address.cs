using NSE.Core.DomainObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSE.Client.API.Models
{
    public class Address : Entity
    {
        public string AddressPlace { get; private set; }

        public string NumberAddress { get; private set; }

        public string Complement { get; private set; }

        public string ZipCode { get; private set; }

        public string Neighborhood { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; private set; }

        //EF Relation
        public Customer Customer { get; protected set; }

        public Address(string addressPlace, string numberAddress, string complement, string zipCode, string neighborhood, string city, string state, Guid customerId)
        {
            AddressPlace = addressPlace;
            NumberAddress = numberAddress;
            Complement = complement;
            ZipCode = zipCode;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            CustomerId = customerId;
        }
    }
}
