namespace PaymentGateway.Models
{
    public class PaymentTransaction
    {
        public PaymentStatus Status { get; set; }
        public long Id { get; set; }
        public Card Card { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string BankTransactionReferenceId { get; set; }
        public string Message { get; set; }
    }

    public class Card
    {
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }

    public enum PaymentStatus
    {
        PaymentAccepted = 1,
        PaymentFailed = 2
    }
}