using System.Threading.Tasks;
using PaymentGateway.Command;
using PaymentGateway.Models;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentCommandRepository
    {
        Task<PaymentTransaction> Save(TransactionResponse transactionResponse, PaymentCommand paymentCommand);
    }
}