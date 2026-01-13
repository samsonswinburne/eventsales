using EventSalesBackend.Data;
using EventSalesBackend.Exceptions.Event;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Repositories.Implementation;

public class EventRepository : IEventRepository
{
    private readonly IMongoCollection<Event> _events;

    public EventRepository(IMongoDbContext context)
    {
        _events = context.Events;
    }

    public async Task<Event> GetByIdAsync(ObjectId id)
    {
        return await _events.Find(e => e.Id == id).FirstOrDefaultAsync();
    }


    public async Task<List<Event>> GetEventsAsync(int page = 0, int pageSize = 10)
    {
        return await _events.Find(_ => true).Limit(pageSize).Skip(page * pageSize).ToListAsync();
    }

    public async Task<bool> UpdateAsync(ObjectId id, UpdateDefinition<Event> updateDefinition)
    {
        var result = await _events.UpdateOneAsync(e => e.Id == id, updateDefinition);
        if (result.MatchedCount == 0) throw new EventNotFoundException(id);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateStatusAsync(ObjectId id, EventStatus status)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Id, id);
        var update = Builders<Event>.Update.Set(e => e.Status, status);
        var result = await _events.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<Event> CreateAsync(Event eventToCreate)
    {
        await _events.InsertOneAsync(eventToCreate);
        return eventToCreate;
    }

    public async Task<bool> DeleteAsync(ObjectId id)
    {
        // hard delete
        var result = await _events.DeleteOneAsync(e => e.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Event>> FindInRadiusByStatusAsync(double latitude, double longitude,
    double radiusMetres = 2000, EventStatus? status = null, int limit = 20, int page = 0)
    {

        var point = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
    new GeoJson2DGeographicCoordinates(longitude, latitude)
);

        var options = new GeoNearOptions<Event, Event>
        {
            DistanceField = "distance",
            MaxDistance = radiusMetres,
            Query = Builders<Event>.Filter.And(
                Builders<Event>.Filter.Eq(e => e.InPersonEvent, true),
                Builders<Event>.Filter.Eq(e => e.Status, EventStatus.Published)
                ),
            Spherical = true
        };

        var results = await _events.Aggregate()
            .GeoNear(point, options)
            .ToListAsync();
        return results;

    }

    public async Task<List<Event>> GetByFilter(FilterDefinition<Event> filter, int limit = 20, int page = 0)
    {
        return await _events.Find(filter).Limit(limit).Skip(limit * page).ToListAsync();
    }

    public async Task<bool> UpdateByFilter(FilterDefinition<Event> filter, UpdateDefinition<Event> update)
    {
        var result = await _events.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> AddAdminToEvents(ObjectId companyId, string userId)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.HostCompanySummary.CompanyId, companyId);
        var update = Builders<Event>.Update.AddToSet(e => e.Admins, userId);

        var result = await _events.UpdateManyAsync(filter, update);
        
        return result.MatchedCount == result.ModifiedCount;
        
    }

    public async Task<bool> RemoveAdminFromEvents(ObjectId companyId, string userId)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.HostCompanySummary.CompanyId, companyId);
        var update = Builders<Event>.Update.Pull(e => e.Admins, userId);

        var result = await _events.UpdateManyAsync(filter, update);
        return result.ModifiedCount > 0;
    }
    public async Task<Event?> GetBySlugProtected(string slug)
    {
        var filter = Builders<Event>.Filter.And(
            Builders<Event>.Filter.Ne(e => e.Status, EventStatus.Draft),
            Builders<Event>.Filter.Eq(e => e.Slug, slug)
            );
        return await _events.Find(filter).FirstOrDefaultAsync();
        
    }

    public async Task<bool> GetSlugTaken(string slug)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Slug, slug);
        return await _events.Find(filter).Limit(1).AnyAsync();
    }
}