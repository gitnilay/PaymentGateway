using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentGateway.Command;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;

namespace PaymentGateway.Adapters
{
    public class BankPaymentProcessor : IPaymentProcessor
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly HttpClient _httpClient;

        public BankPaymentProcessor(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public async Task<TransactionResponse> ProcessPayment(PaymentCommand paymentCommand)
        {
            var httpResponseMessage = await MakePaymentRequest(paymentCommand);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await MapSuccessResponse(httpResponseMessage);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                await MapFailureResponse(paymentCommand, httpResponseMessage);
            }

            throw new Exception(httpResponseMessage.ToString());
        }

        private async Task<HttpResponseMessage> MakePaymentRequest(PaymentCommand paymentCommand)
        {
            var bankPaymentCommand = new BankPaymentCommand
            {
                Amount = paymentCommand.Amount,
                CardNumber = paymentCommand.CardNumber,
                CurrencyCode = paymentCommand.CurrencyCode,
                ExpiryMonth = paymentCommand.ExpiryMonth,
                ExpiryYear = paymentCommand.ExpiryYear,
                Name = paymentCommand.Name
            };

            var content = JsonConvert.SerializeObject(bankPaymentCommand);
            var paymentUri = new Uri($"{_appSettings.Value.BankSimulatorPaymentUrl}/charge");

            return await _httpClient.PostAsync(paymentUri, new StringContent(content, Encoding.UTF8, "application/json"));
        }

        private static async Task MapFailureResponse(PaymentCommand paymentCommand, HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            var failureResponse = JsonConvert.DeserializeObject<BankTransactionFailureResponse>(response);
            throw new PaymentCommandException(paymentCommand, failureResponse.Error, failureResponse.ErrorCode);
        }

        private static async Task<TransactionResponse> MapSuccessResponse(HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            var bankTransactionResponse = JsonConvert.DeserializeObject<BankTransactionResponse>(response);
            return new TransactionResponse
            {
                TransactionId = bankTransactionResponse.BankTransactionId,
                Message = bankTransactionResponse.Message,
                Status = (PaymentStatus) bankTransactionResponse.Status
            };
        }
    }
}