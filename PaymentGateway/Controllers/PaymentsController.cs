using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGateway.Command;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Query;
using PaymentGateway.Utilities;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentCommandHandler _paymentCommandHandler;
        private readonly IPaymentQueryHandler _paymentQueryHandler;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentQueryHandler paymentQueryHandler,
            IPaymentCommandHandler paymentCommandHandler, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _paymentQueryHandler = paymentQueryHandler;
            _paymentCommandHandler = paymentCommandHandler;
            _appSettings = appSettings;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiTransaction), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiFailure), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ChargeAsync([FromBody] PaymentCommand paymentCommand)
        {
            try
            {
                var paymentTransaction = await _paymentCommandHandler.HandleAsync(paymentCommand);
                var uri = $"{_appSettings.Value.PaymentsUrl}/{paymentTransaction.Id}";
                return Created(new Uri(uri), BuildTransactionResponse(paymentTransaction));
            }
            catch (PaymentCommandException exception)
            {
                _logger.LogError(exception, $"Invalid payment request:{exception.PaymentCommand}");
                return BadRequest(new ApiFailure
                {
                    Error = exception.Errors,
                    ErrorCode = exception.ErrorCode
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Internal server error");
                return Problem($"Something went wrong. Client reference:{paymentCommand.CorrelationId}");
            }
        }

        [HttpGet("{paymentId:long}")]
        [ProducesResponseType(typeof(ApiTransaction), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiFailure), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAsync(long paymentId)
        {
            try
            {
                var paymentTransaction = await _paymentQueryHandler.HandleAsync(new PaymentQuery {Id = paymentId});

                if (paymentTransaction != null) return Ok(BuildTransactionResponse(paymentTransaction));

                return NotFound(new ApiFailure
                {
                    Error = "Transaction not found",
                    ErrorCode = 404
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Internal server error for paymentId:{paymentId}");
                return Problem("Something went wrong.", statusCode: (int) HttpStatusCode.InternalServerError);
            }
        }

        private static ApiTransaction BuildTransactionResponse(PaymentTransaction paymentTransaction)
        {
            return new ApiTransaction
            {
                Id = paymentTransaction.Id,
                Amount = paymentTransaction.Amount,
                CurrencyCode = paymentTransaction.CurrencyCode,
                Number = CardMaskingUtility.Mask(paymentTransaction.Card.Number),
                ExpiryMonth = paymentTransaction.Card.ExpiryMonth,
                ExpiryYear = paymentTransaction.Card.ExpiryYear,
                Status = paymentTransaction.Status.ToString(),
                StatusCode = paymentTransaction.Status,
                Message = paymentTransaction.Message
            };
        }
    }
}