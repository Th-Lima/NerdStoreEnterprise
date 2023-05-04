namespace NSE.Payment.API.Models
{
    public enum StatusTransaction
    {
        Authorized = 1,
        Paid,
        Refused,
        Chargedback,
        Cancelled
    }
}
