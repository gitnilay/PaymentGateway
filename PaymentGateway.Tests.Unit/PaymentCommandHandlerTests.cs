using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PaymentGateway.Adapters;
using PaymentGateway.Command;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Validators;
using Xunit;

namespace PaymentGateway.Tests.Unit
{
    public class PaymentCommandHandlerTests
    {
        public PaymentCommandHandlerTests()
        {
            _fixture = new Fixture();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _paymentRepositoryMock = new Mock<IPaymentCommandRepository>();
            var httpClient = new HttpClient(_handlerMock.Object) {BaseAddress = new Uri("http://bank.com")};
            IOptions<AppSettings> appSettings = new OptionsWrapper<AppSettings>(new AppSettings
            {
                BankSimulatorPaymentUrl = "http://bank.com"
            });
            _paymentCommandHandler = new PaymentCommandHandler(new BankPaymentProcessor(httpClient, appSettings),
                new PaymentCommandValidator(), _paymentRepositoryMock.Object);

            _validPaymentCommand = _fixture.Build<PaymentCommand>()
                .With(c => c.CardNumber, "1234567898765432")
                .With(c => c.ExpiryMonth, DateTime.Now.Month)
                .With(c => c.ExpiryYear, DateTime.Now.Year + 1)
                .With(c => c.CurrencyCode, "GBP")
                .Create();
        }

        private readonly IPaymentCommandHandler _paymentCommandHandler;
        private readonly Fixture _fixture;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Mock<IPaymentCommandRepository> _paymentRepositoryMock;
        private readonly PaymentCommand _validPaymentCommand;

        [Fact]
        public async Task Should_BubbleUpException()
        {
            // arrange
            _handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new JsonException("Error transforming json"))
                .Verifiable();

            // act
            // assert
            await Assert.ThrowsAsync<JsonException>(() => _paymentCommandHandler.HandleAsync(_validPaymentCommand));
        }

        [Fact]
        public async Task Should_ProcessPayment()
        {
            // arrange
            var bankTransactionResponse = _fixture.Create<BankTransactionResponse>();
            var paymentTransaction = _fixture.Create<PaymentTransaction>();

            _paymentRepositoryMock.Setup(r =>
                    r.Save(It.IsAny<TransactionResponse>(), It.IsAny<PaymentCommand>()))
                .ReturnsAsync(paymentTransaction);

            MockBankResponse(bankTransactionResponse);

            // act
            var paymentCommandResponse = await _paymentCommandHandler.HandleAsync(_validPaymentCommand);

            // assert
            paymentCommandResponse.Id.Should().Be(paymentTransaction.Id);
            paymentCommandResponse.Status.Should().Be(paymentTransaction.Status);
        }

        [Fact]
        public async Task Should_ValidatePayment()
        {
            // arrange
            // act
            // assert
            await Assert.ThrowsAsync<PaymentCommandException>(() =>
                _paymentCommandHandler.HandleAsync(new PaymentCommand()));
        }

        private void MockBankResponse(BankTransactionResponse bankTransactionResponse)
        {
            _handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(bankTransactionResponse))
                })
                .Verifiable();
        }
    }
}