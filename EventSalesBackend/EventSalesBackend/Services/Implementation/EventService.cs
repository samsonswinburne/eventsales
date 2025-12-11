using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

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

        public async Task<Event> GetByIdAsync(ObjectId id)
        {
            return await _eventRepository.GetByIdAsync(id);
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
