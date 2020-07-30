namespace PaymentGateway.Adapters
{
    public class BankPaymentCommand
    {
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}