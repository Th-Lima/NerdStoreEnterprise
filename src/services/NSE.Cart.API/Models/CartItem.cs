using NSE.Cart.API.Models.Validations;
using System;
using System.Text.Json.Serialization;

namespace NSE.Cart.API.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public Guid CartId { get; set; }

        [JsonIgnore]
        public CartCustomer CartCustomer { get; set; }

        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        internal void JoinCart(Guid cartId)
        {
            CartId = cartId;
        }

        internal decimal CalculateValue()
        {
            return Amount * Price;
        }

        internal void AddUnit(int unit)
        {
            Amount += unit;
        }

        internal void UpdateUnitItem(int unit)
        {
            Amount = unit;
        }

        internal bool IsValid()
        {
            return new CartItemValidation().Validate(this).IsValid;
        }
    }
}
