using System.Collections.Generic;

namespace NSE.Bff.Shopping.Models
{
    public class CartDto
    {
        public decimal TotalValue { get; set; }
        public VoucherDto Voucher { get; set; }
        public bool VoucherUsed { get; set; }
        public decimal Discount { get; set; }
        public List<ItemCartDto> Itens { get; set; } = new List<ItemCartDto>();
    }
}
