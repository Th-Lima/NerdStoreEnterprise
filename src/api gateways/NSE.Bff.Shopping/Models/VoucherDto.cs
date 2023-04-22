namespace NSE.Bff.Shopping.Models
{
    public class VoucherDto
    {
        public decimal? Percentage { get; set; }
        public decimal? ValueDiscount { get; set; }
        public string Code { get; set; }
        public int DiscountType { get; set; }
    }
}
