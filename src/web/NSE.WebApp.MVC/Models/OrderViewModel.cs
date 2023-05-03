using System.Collections.Generic;
using System;

namespace NSE.WebApp.MVC.Models
{
    public class OrderViewModel
    {
        #region Pedido

        public int Code { get; set; }

        // Autorizado = 1,
        // Pago = 2,
        // Recusado = 3,
        // Entregue = 4,
        // Cancelado = 5
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }
        public bool VoucherUsed { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        #endregion

        #region Item Pedido

        public class OrderItemViewModel
        {
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public int Amount { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
        }

        #endregion

        #region Endereco

        public AddressViewModel Address { get; set; }

        #endregion
    }
}
