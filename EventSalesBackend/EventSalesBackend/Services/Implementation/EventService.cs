using EventSalesBackend.Exceptions.MongoDB;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.AdminView.Event;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Services.Implementation;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IGeocodeService _geocodeService;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Event> CreateAsync(Event eventToCreate)
    {
        return await _eventRepository.CreateAsync(eventToCreate);
    }

    public async Task<bool> DeleteAsync(ObjectId id)
    {
        return await _eventRepository.DeleteAsync(id);
    }

    public async Task<List<EventPublic>> FindInRadiusPublicAsync(double latitude, double longitude, double radiusMetres,
        EventStatus? status = null)
    {
        var results = await _eventRepository.FindInRadiusByStatusAsync(latitude, longitude, radiusMetres, status);
        return results.ConvertAll(e => e.ToPublic());
    }

    // this can be combined into 1 big filter query. downsides are that server side validation wouldn't return errors but could be handled client side
    public async Task<bool> MakePublicAsync(ObjectId eventId, string userId)
    {
        var eventIdFilter = Builders<Event>.Filter.Eq(e => e.Id, eventId);
        var eventToSet = await _eventRepository.GetByIdAsync(eventId);

        if (!eventToSet.Admins.Contains(userId))
            throw new UnauthorizedAccessException("You don't have permission to view this draft event");

        if (eventToSet.Description is null)
            // should probably change this to a custom exception
            throw new InvalidOperationException("The event does not have a description");

        if (eventToSet.EndDate.CompareTo(DateTime.UtcNow) >= 0)
            throw new InvalidOperationException("This event has already occured");

        if (eventToSet.TicketTypes.Count == 0)
            throw new InvalidOperationException("The event does not have a ticket type");

        var update = Builders<Event>.Update.Set(e => e.Status, EventStatus.Published);
        var result = await _eventRepository.UpdateAsync(eventToSet.Id, update);
        return result;
    }
    
    public async Task<TicketTypePublic> AddTicketTypeAsync(ObjectId eventId, string userId, TicketType ticketType)
    {
        ticketType.Id = ObjectId.GenerateNewId();
        var eventIdFilter = Builders<Event>.Filter.Eq(e => e.Id, eventId);
        var adminsFilter = Builders<Event>.Filter.AnyEq(e => e.Admins, userId);
        var combinedFilter = Builders<Event>.Filter.And(eventIdFilter, adminsFilter);

        var update = Builders<Event>.Update.AddToSet(e => e.TicketTypes, ticketType);
        var result = await _eventRepository.UpdateByFilter(combinedFilter, update);
        if (!result)
        {
            throw new NotImplementedException();
        }
        return ticketType.ToPublic();
    }

    public async Task<EventPublic> GetByIdPublicAsync(ObjectId id, string userId)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event.Status == EventStatus.Draft && !@event.Admins.Contains(userId))
            throw new UnauthorizedAccessException($"You don't have permission to view this draft event, {@event.Admins[0]}");
        // second part of this exception needs to be removed before going public
        return @event.ToPublic();
    }

    public async Task<Event> GetById(ObjectId id, string userId)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event.Status == EventStatus.Draft && !@event.Admins.Contains(userId))
            throw new UnauthorizedAccessException("You don't have permission to view this draft event");
        return @event;
    }

    public async Task<List<Event>> GetByHost(ObjectId hostId, string userId)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.HostCompanySummary.CompanyId, hostId);
        var events = await _eventRepository.GetByFilter(filter);
        List<Event> eventsToReturn = (from e in events where e.Admins.Contains(userId) select e).ToList();
        return eventsToReturn;
    }

    public async Task<List<EventPublic>> GetByHostPublic(ObjectId hostId)
    {
        var hostIdFilter = Builders<Event>.Filter.Eq(ev => ev.HostCompanySummary.CompanyId, hostId);
        var publicFilter = Builders<Event>.Filter.Ne(ev => ev.Status, EventStatus.Draft);
        var combinedFilter = Builders<Event>.Filter.And(hostIdFilter, publicFilter);

        var events = await _eventRepository.GetByFilter(combinedFilter);

        return events.ConvertAll(e => e.ToPublic());
    }

    public async Task<List<Event>> GetEventsAsync(int page = 0, int pageSize = 10)
    {
        return await _eventRepository.GetEventsAsync(page, pageSize);
    }


    public async Task<bool> UpdateStatusAsync(ObjectId id, EventStatus status)
    {
        return await _eventRepository.UpdateStatusAsync(id, status);
    }

    public async Task<UpdateEventLocationResponse> UpdateEventLocationAsync(ObjectId eventId, string userId, double latitude, double longitude)
    {
        var result = await _geocodeService.ReverseGeocode(latitude, longitude);

        var eventIdFilter = Builders<Event>.Filter.Eq(x => x.Id, eventId);
        var adminsFilter = Builders<Event>.Filter.AnyEq(e => e.Admins, userId);
        var eventTypeFilter = Builders<Event>.Filter.Eq(e => e.InPersonEvent, true);
        var eventStatusFilter = Builders<Event>.Filter.Lte(e => e.Status, EventStatus.Published);

        var combinedFilter = Builders<Event>.Filter.And(eventIdFilter, adminsFilter, eventTypeFilter, eventStatusFilter);
        

        var update = Builders<Event>.Update
        .Set(x => x.VenueLocation,
            new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(latitude, longitude)
            )
        )
        .Set(x => x.VenueAddress, result.Address);

        var updateResult = await _eventRepository.UpdateByFilter(combinedFilter, update);
        if (!updateResult)
        {
            throw new MongoFailedToUpdateException("event");
        }

        // code to update if successful then
        return 
            new UpdateEventLocationResponse
            {
            Address = result.Address
            };
    }

   
}