using System.Threading.Tasks;
using PaymentGateway.Command;
using PaymentGateway.Models;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentCommandHandler
    {
        Task<PaymentTransaction> HandleAsync(PaymentCommand paymentCommand);
    }
}