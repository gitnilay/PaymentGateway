using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Command
{
    public class PaymentCommand
    {
        [Required] 
        public string CorrelationId => Guid.NewGuid().ToString();

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string CurrencyCode { get; set; }
        
        [Required] 
        public decimal Amount { get; set; }

        [Required] 
        public int ExpiryMonth { get; set; }

        [Required] 
        public int ExpiryYear { get; set; }
    }
}