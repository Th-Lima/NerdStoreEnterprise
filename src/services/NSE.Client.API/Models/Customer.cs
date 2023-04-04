using NSE.Core.DomainObjects;
using System;

namespace NSE.Client.API.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public CPF Cpf { get; private set; }
        public bool IsExcluded { get; set; }
        public Address Address { get; private set; }

        //EF Relation
        protected Customer(){}

        public Customer(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new CPF(cpf);
            IsExcluded = false;
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }

        public void AssignAddress(Address address)
        {
            Address = address;
        }
    }
}
