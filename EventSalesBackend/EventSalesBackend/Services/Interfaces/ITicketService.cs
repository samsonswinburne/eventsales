using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface ITicketService
{
    Task<TicketStatus> GetTicketStatusFromKey(string key, string scannerId);
    Task Get(ObjectId id);
}