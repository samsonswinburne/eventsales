using EventSalesBackend.Data;
using EventSalesBackend.Exceptions.Event;
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


    public async Task<Ticket?> Get(ObjectId id, CancellationToken ct)
    {
        return await _tickets.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
    }

    public async Task<bool> Insert(Ticket ticket, CancellationToken ct)
    {
        await _tickets.InsertOneAsync(ticket, ct);
        return ticket.Id != null;
    }

    public async Task<bool> SetStatus(ObjectId ticketId, TicketStatus status, CancellationToken ct)
    {
        var update =  Builders<Ticket>.Update.Set(x => x.Status, status);
        var filter = Builders<Ticket>.Filter.Eq(t => t.Id, ticketId);
        var result = await _tickets.UpdateOneAsync(filter, update, cancellationToken:ct);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<TicketStatus> GetStatusFromKeyProtected(string key, string scannerId, CancellationToken ct)
    {
        // ticket.EventId => lookup eventId => lookup admins
        var ticketKeyFilter = Builders<Ticket>.Filter.Eq(t => t.Key, key);
        

        var result = await _tickets.Aggregate()
            .Match(ticketKeyFilter).Limit(1)
            .Lookup<Ticket, Event>(
                "events", "eventId", "_id", "event"
            ).Unwind("event")
            .FirstOrDefaultAsync(ct);
        if (result is null) return TicketStatus.NotFound;

        if (!result.TryGetValue("status", out var statusBson)) return TicketStatus.NotFound; // ticket has been found
        if (!result.TryGetValue("event", out var eventBson)) throw new EventNotFoundException(); // need to replace this exception, reasonably major business logic error
        // if this is triggered it means that the event has been deleted without the tickets being removed. should be soft delete
        if (!eventBson.ToBsonDocument().TryGetValue("admins", out var adminsBson)) throw new EventNotFoundException(); // need to replace this exception
        
        if (!Enum.IsDefined(typeof(TicketStatus), statusBson.AsInt32))
        {
            Console.WriteLine("this should not occur, there is a bad enum in the database outside the intended range");
            throw new NotImplementedException();
        }

        using (var adminsEnumerator =
               adminsBson.AsBsonArray.GetEnumerator() ?? throw new EventNotFoundException(ObjectId.Empty))
        {
            while (adminsEnumerator.MoveNext()) // could potentially be bad if someone hits the endpoint relentlessly with a long list of admins // should be ratelimited
            {
                string current =  adminsEnumerator.Current.ToString();
                if (!string.IsNullOrEmpty(current) && scannerId.Equals(current))
                {
                    return (TicketStatus)statusBson.AsInt32;
                }
            }
        }
        
        return TicketStatus.NotFound;
    }

    public async Task<bool> UpdateStatusGivenCurrentStatus(string key, string scannerId, TicketStatus statusToSet, TicketStatus? statusRequired, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}