using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;

namespace PaymentGateway.Command
{
    public class PaymentCommandRepository : IPaymentCommandRepository
    {
        private static readonly Random RandomGenerator = new Random();
        private readonly IOptions<AppSettings> _appSettings;

        public PaymentCommandRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<PaymentTransaction> Save(TransactionResponse transactionResponse,
            PaymentCommand paymentCommand)
        {
            var transactionId = RandomGenerator.Next();
            var transaction = new PaymentTransaction
            {
                Amount = paymentCommand.Amount,
                Card = new Card
                {
                    ExpiryMonth = paymentCommand.ExpiryMonth,
                    ExpiryYear = paymentCommand.ExpiryYear,
                    Number = paymentCommand.CardNumber
                },
                CurrencyCode = paymentCommand.CurrencyCode,
                Status = transactionResponse.Status,
                BankTransactionReferenceId = transactionResponse.TransactionId,
                Message = transactionResponse.Message,
                Id = transactionId
            };

            var fileName = Path.Combine(_appSettings.Value.PaymentsDirectoryBasePath, transactionId.ToString());
            await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(transaction));
            return transaction;
        }
    }
}