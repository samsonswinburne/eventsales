using EventSalesBackend.Models;

namespace EventSalesBackend.Repositories.Interfaces;

public interface IHostRepository
{
    Task CreateAsync(User host);
    Task<User?> GetAsync(string hostId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email);
}