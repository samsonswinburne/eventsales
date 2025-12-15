using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IHostService
    {
        Task<ObjectId?> CreateAsync(Host host);
    }
}
