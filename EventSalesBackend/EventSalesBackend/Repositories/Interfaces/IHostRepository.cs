using EventSalesBackend.Models;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IHostRepository
    {
        Task CreateAsync(EventHost host);
        Task<EventHost?> GetAsync(string hostId);
    }
}
