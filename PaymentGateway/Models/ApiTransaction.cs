namespace PaymentGateway.Models
{
    public class ApiTransaction
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        public PaymentStatus StatusCode { get; set; }
        public string Message { get; set; }
    }
}