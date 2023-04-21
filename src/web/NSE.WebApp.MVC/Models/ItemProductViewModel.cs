using System;

namespace NSE.WebApp.MVC.Models
{
    public class ItemProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }
}
