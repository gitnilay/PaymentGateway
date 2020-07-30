using System.Text.RegularExpressions;
using FluentValidation;
using PaymentGateway.Command;

namespace PaymentGateway.Validators
{
    public class PaymentCommandValidator : AbstractValidator<PaymentCommand>
    {
        public PaymentCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("Please provide Name");

            RuleFor(c => c.CardNumber).NotNull().NotEmpty()
                .Matches("^\\d{16}").Length(16).WithMessage("Please provide valid card number");

            RuleFor(c => c.ExpiryMonth).NotNull().NotEmpty().WithMessage("Please provide valid expiry month");

            RuleFor(c => c.ExpiryYear).NotNull().NotEmpty().WithMessage("Please provide valid expiry year");

            RuleFor(c => c.CurrencyCode).NotNull().NotEmpty()
                .Matches("GBP", RegexOptions.IgnoreCase).Length(3).WithMessage("Please provide valid currency code.");

            RuleFor(c => c.Amount).NotNull().NotEmpty().WithMessage("Please provide valid amount");
        }
    }
}