using EventSalesBackend.Data;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Implementation;

public class SessionProvider : ISessionProvider
{
    private readonly IMongoDbContext _dbContext;
    public SessionProvider(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.StartSessionAsync(cancellationToken);
    }
}