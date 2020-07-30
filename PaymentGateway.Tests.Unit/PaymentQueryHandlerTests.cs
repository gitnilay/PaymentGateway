using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Query;
using Xunit;

namespace PaymentGateway.Tests.Unit
{
    public class PaymentQueryHandlerTests
    {
        public PaymentQueryHandlerTests()
        {
            _fixture = new Fixture();
            _paymentQueryRepositoryMock = new Mock<IPaymentQueryRepository>();
            _paymentQueryHandler = new PaymentQueryHandler(_paymentQueryRepositoryMock.Object);
        }

        private readonly Fixture _fixture;
        private readonly IPaymentQueryHandler _paymentQueryHandler;
        private readonly Mock<IPaymentQueryRepository> _paymentQueryRepositoryMock;

        [Fact]
        public async Task Should_BubbleUpException()
        {
            // arrange
            _paymentQueryRepositoryMock.Setup(r => r.GetAsync(It.IsAny<PaymentQuery>())).Throws<Exception>();

            // act
            // assert
            await Assert.ThrowsAsync<Exception>(() => _paymentQueryHandler.HandleAsync(new PaymentQuery()));
        }

        [Fact]
        public async Task Should_GetPaymentTransaction()
        {
            // arrange
            var paymentQuery = _fixture.Create<PaymentQuery>();
            var transaction = _fixture
                .Build<PaymentTransaction>()
                .With(t => t.Id, paymentQuery.Id)
                .Create();

            _paymentQueryRepositoryMock.Setup(r => r.GetAsync(paymentQuery)).ReturnsAsync(transaction);

            // act
            var paymentTransaction = await _paymentQueryHandler.HandleAsync(paymentQuery);

            // assert
            paymentTransaction.Id.Should().Be(paymentQuery.Id);
        }

        [Fact]
        public async Task Should_GetPaymentTransactionWithAmountDetails()
        {
            // arrange
            var paymentQuery = new PaymentQuery();
            var transaction = _fixture.Create<PaymentTransaction>();
            _paymentQueryRepositoryMock.Setup(r => r.GetAsync(paymentQuery)).ReturnsAsync(transaction);

            // act
            var paymentTransaction = await _paymentQueryHandler.HandleAsync(paymentQuery);

            // assert
            paymentTransaction.Amount.Should().Be(transaction.Amount);
            paymentTransaction.CurrencyCode.Should().Be(transaction.CurrencyCode);
        }

        [Fact]
        public async Task Should_GetPaymentTransactionWithCardDetails()
        {
            // arrange
            var paymentQuery = new PaymentQuery();
            var transaction = _fixture.Create<PaymentTransaction>();
            _paymentQueryRepositoryMock.Setup(r => r.GetAsync(paymentQuery)).ReturnsAsync(transaction);

            // act
            var paymentTransaction = await _paymentQueryHandler.HandleAsync(paymentQuery);

            // assert
            paymentTransaction.Card.ExpiryMonth.Should().Be(transaction.Card.ExpiryMonth);
            paymentTransaction.Card.ExpiryYear.Should().Be(transaction.Card.ExpiryYear);
        }

        [Fact]
        public async Task Should_GetPaymentTransactionWithStatus()
        {
            // arrange
            var paymentQuery = new PaymentQuery();
            var transaction = _fixture.Create<PaymentTransaction>();
            _paymentQueryRepositoryMock.Setup(r => r.GetAsync(paymentQuery)).ReturnsAsync(transaction);

            // act
            var paymentTransaction = await _paymentQueryHandler.HandleAsync(paymentQuery);

            // assert
            paymentTransaction.Status.Should().Be(transaction.Status);
        }

        [Fact]
        public async Task Should_HandleInvalidPaymentId()
        {
            // arrange
            // act
            var paymentTransaction = await _paymentQueryHandler.HandleAsync(new PaymentQuery());

            // assert
            paymentTransaction.Should().BeNull();
        }
    }
}