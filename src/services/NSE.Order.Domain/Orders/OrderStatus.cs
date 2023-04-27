namespace NSE.Order.Domain.Orders
{
    public enum OrderStatus
    {
        Authorized = 1,
        PaidOut = 2,
        Refused = 3,
        Delivered = 4,
        Canceled = 5
    }
}
