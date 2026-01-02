using MongoDB.Driver;

namespace EventSalesBackend.Data;

public interface IUnitOfWork : IDisposable
{
    IClientSessionHandle Session { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}