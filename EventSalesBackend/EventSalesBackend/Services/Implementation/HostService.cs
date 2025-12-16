using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Hosts;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Implementation;

public class HostService : IHostService
{
    private readonly IHostRepository _hostRepository;

    public HostService(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;   
    }
    public async Task<bool> CreateHost(CreateHostRequest request, string userId)
    {
        EventHost host = new EventHost
        {
            Id = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            OnBoardingCompleted = false
        };
        try
        {
            await _hostRepository.CreateAsync(host); // i couldnt get output from this
        }
        catch (MongoWriteException)
        {
            // update this it doesn't tell the user why their request failed, just that it failed
            return false;
        }
        return true;
        
        
    }

    public async Task<HostPublic?> GetPublicAsync(string hostId)
    {
        var result = await _hostRepository.GetAsync(hostId);
        if (result is null)
        {
            return null;
        }

        var host = new HostPublic
        {
            FirstName = result.FirstName
        };
        return host;

    }

    public Task<EventHost?> GetAsync(string hostId, string userId)
    {
        return _hostRepository.GetAsync(hostId);
    }
}