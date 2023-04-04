using NSE.Core.DomainObjects;

namespace NSE.Client.API.Models
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }
        public bool IsExcluded { get; set; }
        public Address Address { get; private set; }

        //EF Relation
        protected Client(){}

        public Client(string name, string email, string cpf)
        {
            Name = name;
            Email = email;
            CPF = cpf;
            IsExcluded = false;
        }
    }
}
