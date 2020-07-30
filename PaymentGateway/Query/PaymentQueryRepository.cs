using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentGateway.Interfaces;
using PaymentGateway.Models;

namespace PaymentGateway.Query
{
    public class PaymentQueryRepository : IPaymentQueryRepository
    {
        private readonly IOptions<AppSettings> _appSettings;

        public PaymentQueryRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<PaymentTransaction> GetAsync(PaymentQuery query)
        {
            var fileName = Path.Combine(_appSettings.Value.PaymentsDirectoryBasePath, query.Id.ToString());
            if (!File.Exists(fileName))
                return null;

            var contents = await File.ReadAllTextAsync(fileName);
            return JsonConvert.DeserializeObject<PaymentTransaction>(contents);
        }
    }
}