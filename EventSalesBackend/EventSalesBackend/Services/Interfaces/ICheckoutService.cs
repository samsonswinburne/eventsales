using EventSalesBackend.Models;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Interfaces;

public interface ICheckoutService
{
    Task<List<Ticket>> CreateOrderAsync(string userId, string email, string name, CancellationToken cancellationToken);

}