namespace NSE.Payment.API.Models
{
    public class CreditCard
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string MonthYearDue { get; set; }
        public string CVV { get; set; }

        protected CreditCard() { }

        public CreditCard(string cardName, string cardNumber, string monthYearDue, string cvv)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            MonthYearDue = monthYearDue;
            CVV = cvv;
        }
    }
}
