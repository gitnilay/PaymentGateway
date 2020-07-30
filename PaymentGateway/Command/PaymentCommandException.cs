using System;

namespace PaymentGateway.Command
{
    public class PaymentCommandException : Exception
    {
        public readonly int ErrorCode;
        public readonly string Errors;
        public readonly PaymentCommand PaymentCommand;

        public PaymentCommandException(PaymentCommand paymentCommand, string errorMessage, int errorCode)
        {
            PaymentCommand = paymentCommand;
            Errors = errorMessage;
            ErrorCode = errorCode;
        }
    }
}