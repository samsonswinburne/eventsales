using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IHostRepository
    {
        Task CreateAsync(EventHost host);

    }
}
