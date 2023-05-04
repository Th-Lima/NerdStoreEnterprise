using NSE.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace NSE.Payment.API.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment()
        {
            Transactions = new List<Transaction>();
        }

        public Guid OrderId { get; set; }
        public TypePayment TypePayment { get; set; }
        public decimal TotalValue { get; set; }

        public CreditCard CreditCard { get; set; }

        // EF Relation
        public ICollection<Transaction> Transactions { get; set; }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}
