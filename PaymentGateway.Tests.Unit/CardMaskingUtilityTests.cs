using FluentAssertions;
using PaymentGateway.Utilities;
using Xunit;

namespace PaymentGateway.Tests.Unit
{
    public class CardMaskingUtilityTests
    {
        [Fact]
        public void Should_MaskSixNumbersOfSixteenDigitCardNumber()
        {
            // arrange
            var expectedCardNumber = "012345******6543";
            var cardNumber = "0123456789876543";

            // act
            var maskedNumber = CardMaskingUtility.Mask(cardNumber);

            // assert
            maskedNumber.Should().Be(expectedCardNumber);
        }
    }
}