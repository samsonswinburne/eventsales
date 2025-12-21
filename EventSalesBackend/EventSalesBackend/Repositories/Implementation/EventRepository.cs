using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation
{
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
        public async Task<List<Event>> FindInRadiusByStatusAsync(double latitude, double longitude, double radiusMetres = 2000, EventStatus? status = null, int limit = 20, int page = 0)
        {
            var sort = Builders<Event>.Sort.Descending(e => e.Summary.TotalSold);
            var statusFilter = Builders<Event>.Filter.Eq(e => e.Status, EventStatus.Published);
            if (status != null)
            {
                statusFilter = Builders<Event>.Filter.Eq(e => e.Status, status);
            }
            var locationFilter = Builders<Event>.Filter.Near(e => e.VenueLocation, latitude, longitude, radiusMetres);
            var statusLocationFilter = Builders<Event>.Filter.And(statusFilter, locationFilter);

            return await _events.Find(statusLocationFilter).Sort(sort).Limit(limit).Skip(page*limit).ToListAsync();
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
    }
}
