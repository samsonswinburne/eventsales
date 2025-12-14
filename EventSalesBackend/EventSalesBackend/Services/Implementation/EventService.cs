using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
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

        public async Task<List<Event>> FindInRadiusByStatusAsync(double latitude, double longitude, double radiusMetres, EventStatus? status = null)
        {
            return await _eventRepository.FindInRadiusByStatusAsync(latitude, longitude, radiusMetres, status);
        }

        public async Task<GetEventPublicResponse> GetByIdPublicAsync(ObjectId id, string userId)
        {
            var @event = await _eventRepository.GetByIdAsync(id);
            if (@event.Status == EventStatus.Draft && !@event.Admins.Contains(userId))
            {
                throw new UnauthorizedAccessException("You don't have permission to view this draft event");
            }
            return @event.ToPublic();
        }
        public async Task<Event> GetById(ObjectId id, string userId)
        {
            var @event = await _eventRepository.GetByIdAsync(id);
            if (@event.Status == EventStatus.Draft && !@event.Admins.Contains(userId))
            {
                throw new UnauthorizedAccessException("You don't have permission to view this draft event");
            }
            return @event;
        }
        public async Task<List<Event>> GetByHost(ObjectId hostId, string userId)
        {
            var filter = Builders<Event>.Filter.Eq(e => e.HostCompanySummary.CompanyId, hostId);
            var events = await _eventRepository.GetByFilter(filter);
            List<Event> eventsToReturn = (from e in events where e.Admins.Contains(userId) select e).ToList();
            return eventsToReturn;
        }
        public async Task<List<GetEventPublicResponse>> GetByHostPublic(ObjectId hostId)
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

        public async Task<bool> UpdateAsync(ObjectId id, Event eventUpdate)
        {
            return await _eventRepository.UpdateAsync(id, eventUpdate);
        }

        public async Task<bool> UpdateStatusAsync(ObjectId id, EventStatus status)
        {
            return await _eventRepository.UpdateStatusAsync(id, status);
        }
        
    }
}
