using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Query;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentQueryHandler
    {
        Task<PaymentTransaction> HandleAsync(PaymentQuery query);
    }
}