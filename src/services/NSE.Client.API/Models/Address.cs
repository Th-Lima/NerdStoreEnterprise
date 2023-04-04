using NSE.Core.DomainObjects;
using System;

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

        public Guid ClientId { get; private set; }

        //EF Relation
        public Client Client { get; protected set; }

        public Address(string addressPlace, string numberAddress, string complement, string zipCode, string neighborhood, string city, string state)
        {
            AddressPlace = addressPlace;
            NumberAddress = numberAddress;
            Complement = complement;
            ZipCode = zipCode;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
