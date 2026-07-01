using MongoDB.Driver;

namespace EventSalesBackend.Services.Interfaces;

public interface ISessionProvider
{
    Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default);
}