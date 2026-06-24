using EventSalesBackend.Models;
using EventSalesBackend.Services.Interfaces;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Models;

namespace EventSalesBackend.Services.Implementation;

public class PayPalService : IPayPalService
{
    private readonly PaypalServerSdkClient _client;
    public PayPalService(IPayPalClientService clientService)
    {
        _client = clientService.Client;
    }
    public async Task<string?> CreateOrder(List<Ticket> tickets, CancellationToken ct)
    {
        var order = new CreateOrderInput
        {
            Body = new OrderRequest
            {
                Intent = CheckoutPaymentIntent.Authorize, // authorise instead of capture so we can reject payments that take too long
                PurchaseUnits = tickets.ConvertAll(t => t.ToPurchaseUnitRequest()),
            }
        };
        var response = await _client.OrdersController.CreateOrderAsync(order, ct);
        
        if (response.StatusCode == 200)
        {
            return response.Data.Id;
        }
        else
        {
            return null;
        }
    }
}