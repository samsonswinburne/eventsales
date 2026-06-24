using EventSalesBackend.Models;

namespace EventSalesBackend.Services.Interfaces;

public interface IPayPalService
{
    public Task<string?> CreateOrder(List<Ticket> tickets, CancellationToken ct);
}