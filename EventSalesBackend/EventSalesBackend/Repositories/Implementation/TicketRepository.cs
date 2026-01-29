using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Ticket> _tickets;
    
    public TicketRepository(IMongoDbContext dbContext)
    {
        _tickets = dbContext.Tickets;
    }


    public async Task<Ticket?> Get(ObjectId id)
    {
        return await _tickets.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> Insert(Ticket ticket)
    {
        await _tickets.InsertOneAsync(ticket);
        return ticket.Id != null;
    }

    public async Task<bool> SetStatus(ObjectId ticketId, TicketStatus status)
    {
        var update =  Builders<Ticket>.Update.Set(x => x.Status, status);
        var result = await _tickets.UpdateOneAsync(ticket => ticket.Id == ticketId, update);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<TicketStatus?> GetStatusFromScan(string key, string scannerId)
    {
        // ticket.EventId => lookup eventId => lookup admins
        var scannerIdFilter = Builders<Event>.Filter.AnyEq(e => e.Admins, scannerId);
        var ticketKeyFilter = Builders<Ticket>.Filter.Eq(t => t.Key, key);
        

        var result = await _tickets.Aggregate()
            .Match(ticketKeyFilter).Limit(1)
            .Lookup<Ticket, Event>(
                "events", "eventId", "_id", "event"
            ).Match(scannerIdFilter)
            .As<Ticket>()
            .Project(t => t.Status)
            .FirstOrDefaultAsync();
        
        return result;
    }

    public async Task<bool> UpdateStatusGivenCurrentStatus(string key, string scannerId, TicketStatus statusToSet, TicketStatus? statusRequired)
    {
        throw new NotImplementedException();
    }
}