using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Cart.API.Models
{
    public class CartCustomer
    {
        internal const int MAX_AMOUNT_ITEM = 5;

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

        internal void CalculateCartValue()
        {
            TotalValue = Itens.Sum(x => x.CalculateValue());
        }

        internal bool CartItemAlreadyExists(CartItem item)
        {
            return Itens.Any(x => x.ProductId == item.ProductId);
        }

        internal CartItem GetByProductId(Guid productId) 
        { 
            return Itens.FirstOrDefault(x => x.ProductId == productId); 
        }

        internal void AddItem(CartItem item)
        {
            if (!item.IsValid())
                return;

            item.JoinCart(Id);

            if (CartItemAlreadyExists(item))
            {
                var itemAlreadyExists = GetByProductId(item.ProductId);
                itemAlreadyExists.AddUnit(item.Amount);

                item = itemAlreadyExists;
                Itens.Remove(itemAlreadyExists);

            }

            Itens.Add(item);
            CalculateCartValue();
        }
    }
}
