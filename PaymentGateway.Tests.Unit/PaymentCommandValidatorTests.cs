using System;
using AutoFixture;
using FluentAssertions;
using PaymentGateway.Command;
using PaymentGateway.Validators;
using Xunit;

namespace PaymentGateway.Tests.Unit
{
    public class PaymentCommandValidatorTests
    {
        public PaymentCommandValidatorTests()
        {
            var fixture = new Fixture();
            _validator = new PaymentCommandValidator();
            _validPaymentCommand = fixture.Build<PaymentCommand>()
                .With(c => c.CardNumber, "1234567898765432")
                .With(c => c.ExpiryMonth, DateTime.Now.Month)
                .With(c => c.ExpiryYear, DateTime.Now.Year + 1)
                .With(c => c.CurrencyCode, "GBP")
                .Create();
        }

        private readonly PaymentCommandValidator _validator;
        private readonly PaymentCommand _validPaymentCommand;

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_ValidateCardHolderName(string name)
        {
            // arrange
            _validPaymentCommand.Name = name;

            // act
            var validationResult = _validator.Validate(_validPaymentCommand);

            // assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("xyz")]
        [InlineData("xyz223424")]
        [InlineData("234*!432324")]
        [InlineData("012345678987654321")]
        [InlineData("012345678987")]
        [InlineData("012345")]
        public void Should_ValidateCardNumberFormat(string cardNumber)
        {
            // arrange
            _validPaymentCommand.CardNumber = cardNumber;

            // act
            var validationResult = _validator.Validate(_validPaymentCommand);

            // assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("xyz")]
        public void Should_ValidateCurrencyCode(string currencyCode)
        {
            // arrange
            _validPaymentCommand.CurrencyCode = currencyCode;

            // act
            var validationResult = _validator.Validate(_validPaymentCommand);

            // assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_AcceptValidPaymentCommand()
        {
            // arrange
            // act
            var validationResult = _validator.Validate(_validPaymentCommand);

            // assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}