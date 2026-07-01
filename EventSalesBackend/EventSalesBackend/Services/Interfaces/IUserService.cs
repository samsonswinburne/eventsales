using EventSalesBackend.Models;

namespace EventSalesBackend.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdOrEmailAsync(string userId, string email, CancellationToken cancellationToken);
}