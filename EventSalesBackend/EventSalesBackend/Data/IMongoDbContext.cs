using EventSalesBackend.Models;
using MongoDB.Driver;

namespace EventSalesBackend.Data;

public interface IMongoDbContext
{
    IMongoCollection<Event> Events { get; }
    IMongoCollection<EventHost> Hosts { get; }
    IMongoCollection<Company> Companies { get; }
    IMongoCollection<Ticket> Tickets { get; }
    IMongoCollection<RequestCompanyAdmin> CompanyAdminRequests { get; }
    IClientSessionHandle StartSession();
}