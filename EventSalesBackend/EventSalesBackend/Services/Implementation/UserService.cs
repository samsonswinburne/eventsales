using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;

namespace EventSalesBackend.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<User?> GetByIdOrEmailAsync(string userId, string email, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdOrEmailAsync(userId, email, cancellationToken);
    }
}