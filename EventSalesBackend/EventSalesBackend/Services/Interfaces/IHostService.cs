using EventSalesBackend.Models.DTOs.Request;

namespace EventSalesBackend.Services.Interfaces;

public interface IHostService
{
    Task<bool> CreateHost(CreateHostRequest request, string userId);
}