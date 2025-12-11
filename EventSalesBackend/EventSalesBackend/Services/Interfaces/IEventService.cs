using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync(int page = 0, int pageSize = 10);
        Task<GetEventPublicResponse> GetByIdPublicAsync(ObjectId id, ObjectId userId);
        Task<bool> UpdateAsync(ObjectId id, Event eventUpdate);
        Task<bool> UpdateStatusAsync(ObjectId id, EventStatus status);
        Task<Event> CreateAsync(Event eventToCreate);
        Task<bool> DeleteAsync(ObjectId id);
        Task<List<Event>> FindInRadiusByStatusAsync(double latitude, double longitude, double radiusMetres, EventStatus? status = null);
    }
}
