namespace PaymentGateway.Adapters
{
    public class BankTransactionFailureResponse
    {
        public string Error { get; set; }
        public int ErrorCode { get; set; }
    }
}