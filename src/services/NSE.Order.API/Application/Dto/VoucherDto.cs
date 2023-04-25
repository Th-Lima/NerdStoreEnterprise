namespace NSE.Order.API.Application.Dto
{
    public class VoucherDto
    {
        public string Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? ValueDiscount { get; set; }
        public int DiscountType { get; set; }
    }
}
