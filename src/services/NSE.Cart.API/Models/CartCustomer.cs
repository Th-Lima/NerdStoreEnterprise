using System;
using System.Collections.Generic;

namespace NSE.Cart.API.Models
{
    public class CartCustomer
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> Itens { get; set; } = new List<CartItem>();

        public CartCustomer(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CartCustomer() { }
    }
}
