using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Query
{
    public class PaymentQuery
    {
        [Required] public long Id { get; set; }
    }
}