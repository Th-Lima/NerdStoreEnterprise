using NSE.Core.DomainObjects;
using System;

namespace NSE.Payment.API.Models
{
    public class Transaction : Entity
    {
        public string AuthorizationCode { get; set; }
        public string CardBrand { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TransactionCost { get; set; }
        public StatusTransaction Status { get; set; }
        public string TID { get; set; } // Id
        public string NSU { get; set; } // Meio (paypal)

        public Guid PaymentId { get; set; }

        // EF Relation
        public Payment Payment { get; set; }
    }
}
