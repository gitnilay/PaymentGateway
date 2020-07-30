using System;
using System.Net;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Adapters;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]/api/v1")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BankSimulatorController : ControllerBase
    {
        private static readonly Fixture Fixture = new Fixture();

        [HttpPost("Charge")]
        [ProducesResponseType(typeof(BankTransactionResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BankTransactionFailureResponse), (int) HttpStatusCode.BadRequest)]
        public ActionResult<BankTransactionResponse> Charge([FromBody] BankPaymentCommand paymentCommand)
        {
            DateTime.TryParse($"01/{paymentCommand.ExpiryMonth}/{paymentCommand.ExpiryYear}",
                out var dateTime);

            if (dateTime.Date < DateTimeOffset.UtcNow.Date)
                return BadRequest(new BankTransactionFailureResponse
                {
                    Error = "Invalid expiry month/year",
                    ErrorCode = 1001
                });

            var paymentStatus = Fixture.Create<bool>() ? BankPaymentStatus.PaymentAccepted : BankPaymentStatus.PaymentFailed;

            return Ok(new BankTransactionResponse
            {
                BankTransactionId = Fixture.Create<string>(),
                Message = paymentStatus == BankPaymentStatus.PaymentAccepted ? string.Empty : "Not enough money",
                Status = paymentStatus
            });
        }
    }
}