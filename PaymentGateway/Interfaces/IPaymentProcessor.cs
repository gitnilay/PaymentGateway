using System.Threading.Tasks;
using PaymentGateway.Command;
using PaymentGateway.Models;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentProcessor
    {
        Task<TransactionResponse> ProcessPayment(PaymentCommand paymentCommand);
    }
}