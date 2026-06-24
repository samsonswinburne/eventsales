using EventSalesBackend.Services.Interfaces;
using PaypalServerSdk.Standard;
using Environment = PaypalServerSdk.Standard.Environment;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace EventSalesBackend.Services.Implementation;

public class PayPalClientService : IPayPalClientService
{
    
    private readonly PaypalServerSdkClient _client;
    
    public PayPalClientService(IConfiguration configuration)
    {
        _client = PaypalServerSdkClient.Builder
            .FromConfiguration(configuration.GetSection("PaypalServerSdk"))
            .Build();
    }
    
    public PaypalServerSdkClient Client => _client;
}