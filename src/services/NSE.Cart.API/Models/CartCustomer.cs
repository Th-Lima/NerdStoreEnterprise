using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using NSE.Cart.API.Models.Validations;

namespace NSE.Cart.API.Models
{
    public class CartCustomer
    {
        internal const int MAX_AMOUNT_ITEM = 5;

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> Itens { get; set; } = new List<CartItem>();

        public ValidationResult ValidationResult { get; set; }

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

        internal void UpdateItem(CartItem item)
        {
            item.JoinCart(Id);

            var itemExists = GetByProductId(item.ProductId);

            Itens.Remove(itemExists);
            Itens.Add(item);

            CalculateCartValue();
        }

        internal void RemoveItem(CartItem item)
        {
            Itens.Remove(GetByProductId(item.ProductId));

            CalculateCartValue();
        }

        internal void UpdateUnit(CartItem item, int unit)
        {
            item.UpdateUnitItem(unit);
            UpdateItem(item);
        }

        internal bool IsValid()
        {
            var errors = Itens.SelectMany(x => new CartItemValidation().Validate(x).Errors).ToList();
            errors.AddRange(new CartCustomerValidation().Validate(this).Errors);

            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }
    }
}
