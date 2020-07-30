using PaymentGateway.Models;

namespace PaymentGateway.Adapters
{
    public class BankTransactionResponse
    {
        public string BankTransactionId { get; set; }
        public BankPaymentStatus Status { get; set; }
        public string Message { get; set; }
    }
}