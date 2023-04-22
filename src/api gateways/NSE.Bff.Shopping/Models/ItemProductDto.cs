using System;

namespace NSE.Bff.Shopping.Models
{
    public class ItemProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int StockAmount { get; set; }
    }
}
