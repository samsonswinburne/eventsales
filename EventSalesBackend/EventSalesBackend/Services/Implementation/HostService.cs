using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request;
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
            await _hostRepository.CreateAsync(host);
        }
        catch (MongoWriteException ex)
        {
            return false;
        }
        return true;
        
        
    }
}