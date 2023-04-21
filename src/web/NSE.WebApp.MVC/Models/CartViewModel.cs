﻿using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal TotalValue { get; set; }
        public List<ItemProductViewModel> Itens { get; set; } = new List<ItemProductViewModel>();
    }
}