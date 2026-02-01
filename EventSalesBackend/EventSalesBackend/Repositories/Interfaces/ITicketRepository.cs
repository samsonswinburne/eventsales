using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> Get(ObjectId id, CancellationToken cancellationToken);
        Task<bool> Insert(Ticket ticket, CancellationToken cancellationToken);
        Task<bool> SetStatus(ObjectId ticketId, TicketStatus status, CancellationToken cancellationToken);
        Task<TicketStatus> GetStatusFromKeyProtected(string key, string scannerId, CancellationToken cancellationToken);
        Task<bool> UpdateStatusGivenCurrentStatus(string key, string scannerId, TicketStatus statusToSet, TicketStatus? statusRequired, CancellationToken cancellationToken);

        Task<bool> UpdateStatusByKey(string key, TicketStatus status,
            CancellationToken ct);

    }
}
