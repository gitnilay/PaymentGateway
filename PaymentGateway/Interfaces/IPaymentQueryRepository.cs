using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Query;

namespace PaymentGateway.Interfaces
{
    public interface IPaymentQueryRepository
    {
        Task<PaymentTransaction> GetAsync(PaymentQuery query);
    }
}