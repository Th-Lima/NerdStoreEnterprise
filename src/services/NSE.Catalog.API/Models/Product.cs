using NSE.Core.DomainObjects;
using System;

namespace NSE.Catalog.API.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Image { get; set; }
        public int StockAmount { get; set; }
    }
}
