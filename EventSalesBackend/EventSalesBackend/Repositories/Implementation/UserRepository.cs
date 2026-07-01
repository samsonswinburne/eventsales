using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDbContext mongoDbContext)
    {
        _users = mongoDbContext.Users;
    }
    public async Task<User?> GetByIdOrEmailAsync(string userId, string email, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq("_id", userId),
            Builders<User>.Filter.Eq(u => u.Email, email)
        );
        return await _users.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}