using System;

namespace NSE.Bff.Shopping.Models
{
    public class ItemCartDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
    }
}
