using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class HostRepository : IHostRepository
{
    private readonly IMongoCollection<User> _hosts;

    public HostRepository(IMongoDbContext context)
    {
        _hosts = context.Users;
    }

    public async Task CreateAsync(User host)
    {
        await _hosts.InsertOneAsync(host);
    }

    public async Task<User?> GetAsync(string hostId, CancellationToken ct)
    {
        return await _hosts.Find(x => x.Id == hostId).FirstOrDefaultAsync(ct);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _hosts.Find(x => x.Email == email).FirstOrDefaultAsync();
    }
}