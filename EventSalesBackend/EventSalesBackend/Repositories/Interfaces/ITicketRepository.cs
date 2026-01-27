using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        public Task<Ticket?> Get(ObjectId id);
        public Task<bool> Insert(Ticket ticket);
    }
}
