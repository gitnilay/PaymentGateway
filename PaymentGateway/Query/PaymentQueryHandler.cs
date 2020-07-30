using System.Threading.Tasks;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;

namespace PaymentGateway.Query
{
    public class PaymentQueryHandler : IPaymentQueryHandler
    {
        private readonly IPaymentQueryRepository _paymentQueryRepository;

        public PaymentQueryHandler(IPaymentQueryRepository paymentQueryRepository)
        {
            _paymentQueryRepository = paymentQueryRepository;
        }

        public async Task<PaymentTransaction> HandleAsync(PaymentQuery query)
        {
            return await _paymentQueryRepository.GetAsync(query);
        }
    }
}