using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class HostRepository : IHostRepository
{
    private readonly IMongoCollection<EventHost> _hosts;

    public HostRepository(IMongoDbContext context)
    {
        _hosts = context.Hosts;
    }

    public async Task CreateAsync(EventHost host)
    {
        await _hosts.InsertOneAsync(host);
    }

    public async Task<EventHost?> GetAsync(string hostId)
    {
        return await _hosts.Find(x => x.Id == hostId).FirstOrDefaultAsync();
    }

    public async Task<EventHost?> GetByEmailAsync(string email)
    {
        return await _hosts.Find(x => x.Email == email).FirstOrDefaultAsync();
    }
}