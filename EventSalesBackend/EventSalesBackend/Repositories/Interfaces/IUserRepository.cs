using EventSalesBackend.Models;

namespace EventSalesBackend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdOrEmailAsync(string userId, string email);
}