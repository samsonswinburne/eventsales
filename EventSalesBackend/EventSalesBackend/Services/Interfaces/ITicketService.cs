using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Interfaces;

public interface ITicketService
{
    Task<TicketStatus> GetTicketStatusFromKey(string key, string scannerId, CancellationToken cancellationToken);
    Task Get(ObjectId id, CancellationToken cancellationToken);

    Task<bool> InsertMany(List<Ticket> tickets, IClientSessionHandle handle, CancellationToken cancellationToken);

    Task<TicketStatus> UpdateStatusByKeyProtected(string key, TicketStatus status, string scannerId, bool overrideLogic,
        CancellationToken cancellationToken);
}