using MongoDB.Driver;

namespace EventSalesBackend.Data;

public class MongoUnitOfWork : IUnitOfWork
{
    private readonly IMongoDbContext _context;
    public IClientSessionHandle Session { get; }
    
    public MongoUnitOfWork(IMongoDbContext context)
    {
        _context = context;
        Session = _context.StartSession();
    }
    
    public Task BeginTransactionAsync()
    {
        Session.StartTransaction();
        return Task.CompletedTask;
    }
    
    public Task CommitAsync() => Session.CommitTransactionAsync();
    
    public Task RollbackAsync() => Session.AbortTransactionAsync();
    
    public void Dispose() => Session?.Dispose();
}