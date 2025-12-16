using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation
{
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
    }
}
