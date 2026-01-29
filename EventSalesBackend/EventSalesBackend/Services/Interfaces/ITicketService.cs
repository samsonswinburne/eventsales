using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface ITicketService
{
    Task<TicketStatus> GetTicketStatusFromKey(string key, string scannerId, CancellationToken cancellationToken);
    Task Get(ObjectId id, CancellationToken cancellationToken);

    Task<TicketPublic> CreateTicket(ObjectId eventId, ObjectId ticketTypeId, ObjectId? customerId,
        string customerEmail, string customerName, string? customerPhone, ICryptoService crypto);
}