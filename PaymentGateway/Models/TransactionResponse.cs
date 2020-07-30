namespace PaymentGateway.Models
{
    public class TransactionResponse
    {
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string Message { get; set; }
    }
}