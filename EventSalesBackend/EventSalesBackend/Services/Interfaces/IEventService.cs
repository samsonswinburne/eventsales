using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.AdminView.Event;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface IEventService
{
    Task<List<Event>> GetEventsAsync(int page = 0, int pageSize = 10);
    Task<EventPublic> GetByIdPublicAsync(ObjectId id, string userId);
    Task<Event> GetById(ObjectId id, string userId);
    Task<List<Event>> GetByHost(ObjectId hostId, string userId);
    Task<List<EventPublic>> GetByHostPublic(ObjectId hostId);
    Task<bool> UpdateStatusAsync(ObjectId id, EventStatus status);
    Task<Event> CreateAsync(Event eventToCreate);
    Task<bool> DeleteAsync(ObjectId id);

    Task<List<EventPublic>> FindInRadiusPublicAsync(double latitude, double longitude, double radiusMetres,
        EventStatus? status = null);

    Task<bool> MakePublicAsync(ObjectId eventId, string userId, string slug);
    Task<TicketTypePublic> AddTicketTypeAsync(ObjectId eventId, string userId, TicketType ticketType);

    Task<UpdateEventLocationResponse> UpdateEventLocationAsync(ObjectId eventId, string userId, double latitude, double longitude);

    Task<bool> AddAdminToEvents(ObjectId companyId, string userId);
    Task<bool> RemoveAdminFromEvents(ObjectId companyId, string userId);
    Task<EventPublic?> GetBySlugPublicProtected(string slug);
    Task<bool> GetSlugAvailable(string slug);
}