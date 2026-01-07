using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Hosts;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface IHostService
{
    Task<bool> CreateHost(CreateHostRequest request, string userId, string email);
    Task<HostPublic?> GetPublicAsync(string hostId);
    Task<EventHost?> GetAsync(string hostId, string userId);
    Task<EventHost?> GetByEmailAsync(string email);
    Task<bool> AcceptRcaAsync(ObjectId rcaId, string userId);
    Task<bool> DeclineRcaAsync(ObjectId rcaId, string userId);
    Task<RequestCompanyAdminPublic> GetRcaByIdAsyncProtected(ObjectId id, string hostId);
    Task<List<RequestCompanyAdminPublic>> GetRcaByHostIdStatusAsync(string hostId, RcaStatus? status);
}