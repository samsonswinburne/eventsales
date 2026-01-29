using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> Get(ObjectId id);
        Task<bool> Insert(Ticket ticket);
        Task<bool> SetStatus(ObjectId ticketId, TicketStatus status);
        Task<TicketStatus?> GetStatusFromScan(string key, string scannerId);
        Task<bool> UpdateStatusGivenCurrentStatus(string key, string scannerId, TicketStatus statusToSet, TicketStatus? statusRequired);
        
    }
}
