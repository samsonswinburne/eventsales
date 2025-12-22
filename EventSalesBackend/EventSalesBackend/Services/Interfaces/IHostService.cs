using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Hosts;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;

namespace EventSalesBackend.Services.Interfaces;

public interface IHostService
{
    Task<bool> CreateHost(CreateHostRequest request, string userId);
    Task<HostPublic?> GetPublicAsync(string hostId);
    Task<EventHost?> GetAsync(string hostId, string userId);
}