using PaypalServerSdk.Standard;

namespace EventSalesBackend.Services.Interfaces;

public interface IPayPalClientService
{
    public PaypalServerSdkClient Client { get; }
}