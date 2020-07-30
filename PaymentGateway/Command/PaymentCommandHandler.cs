using System.Threading.Tasks;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Validators;

namespace PaymentGateway.Command
{
    public class PaymentCommandHandler : IPaymentCommandHandler
    {
        private readonly IPaymentCommandRepository _paymentCommandRepository;
        private readonly PaymentCommandValidator _paymentCommandValidator;
        private readonly IPaymentProcessor _paymentProcessor;

        public PaymentCommandHandler(IPaymentProcessor paymentProcessor,
            PaymentCommandValidator paymentCommandValidator,
            IPaymentCommandRepository paymentCommandRepository)
        {
            _paymentProcessor = paymentProcessor;
            _paymentCommandValidator = paymentCommandValidator;
            _paymentCommandRepository = paymentCommandRepository;
        }

        public async Task<PaymentTransaction> HandleAsync(PaymentCommand paymentCommand)
        {
            var validationResult = await _paymentCommandValidator.ValidateAsync(paymentCommand);

            if (!validationResult.IsValid)
                throw new PaymentCommandException(paymentCommand, validationResult.ToString(), 1002);

            var transactionResponse = await _paymentProcessor.ProcessPayment(paymentCommand);

            return await _paymentCommandRepository.Save(transactionResponse, paymentCommand);
        }
    }
}