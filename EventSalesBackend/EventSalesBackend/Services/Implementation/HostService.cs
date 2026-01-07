using EventSalesBackend.Exceptions.MongoDB;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Hosts;
using EventSalesBackend.Models.DTOs.Request.Hosts;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.Metrics;

namespace EventSalesBackend.Services.Implementation;

public class HostService : IHostService
{
    private readonly IHostRepository _hostRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IRequestCompanyAdminRepository _requestCompanyAdminRepository;
    private readonly IEventRepository _eventRepository;

    public HostService(IHostRepository hostRepository, ICompanyRepository companyService, IRequestCompanyAdminRepository requestCompanyAdminRepository, IEventRepository eventService)
    {
        _hostRepository = hostRepository;
        _companyRepository = companyService;
        _requestCompanyAdminRepository = requestCompanyAdminRepository;
        _eventRepository = eventService;
    }

    public async Task<bool> CreateHost(CreateHostRequest request, string userId, string email)
    {

        if (email is null || email.Length < 3)
        {
            throw new ArgumentException("email, this shouldn't occur. It should be validated in the controller");
        }
        var host = new EventHost
        {
            Id = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            OnBoardingCompleted = false,
            Email = email
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
        if (result is null) return null;

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

    public Task<EventHost?> GetByEmailAsync(string email)
    {
        return _hostRepository.GetByEmailAsync(email);
    }

     // should be moved later
    private async Task<HostRollbackOperationResult> RunRollback(
    HostRollbackOperation name,
    Func<Task<bool>> operation)
    {
        try
        {
            var success = await operation();
            return new HostRollbackOperationResult(name, success);
        }
        catch (Exception ex)
        {
            // log ex here
            return new HostRollbackOperationResult(name, false);
        }
    }
    public async Task<bool> AcceptRcaAsync(ObjectId rcaId, string userId)
    {
        var updateRcaTask =  _requestCompanyAdminRepository.UpdateAsyncProtected(rcaId, userId, RcaStatus.Approved);
        var getRcaTask =  _requestCompanyAdminRepository.GetAsyncProtected(rcaId, userId);

        await Task.WhenAll(updateRcaTask);

        var updateRcaResult = await updateRcaTask;
        var getRcaResult = await getRcaTask;

        if (!updateRcaResult) throw new MongoFailedToUpdateException("status");
        if (getRcaResult?.Id == null) throw new MongoNotFoundException("rca");
        var companyId = getRcaResult.CompanyId;
        
        var addToCompanyTask = _companyRepository.AddCompanyAdminAsync(getRcaResult.CompanyId, userId);
        var addToEventsTask = _eventRepository.AddAdminToEvents(getRcaResult.CompanyId, userId);

        await Task.WhenAll(addToCompanyTask, addToEventsTask);
        var addToCompanyResult = await addToCompanyTask;
        var addToEventsResult = await addToEventsTask;

        var retries = 0;

        if (addToCompanyResult && addToEventsResult) return true; // succesful run should end here - otherwise roll back

        var rollbackRcaPending = true;
        var rollbackCompanyPending = addToCompanyResult;
        var rollbackEventsPending = addToEventsResult;

        while (rollbackRcaPending || rollbackCompanyPending || rollbackEventsPending)
        {
            var rollbackTasks = new List<Task<HostRollbackOperationResult>>();

            if (rollbackRcaPending)
            {
                rollbackTasks.Add(
                    RunRollback(
                        HostRollbackOperation.Rca,
                        () => _requestCompanyAdminRepository
                            .RollbackAsync(rcaId, userId)
                    )
                );
            }

            if (rollbackCompanyPending)
            {
                rollbackTasks.Add(
                    RunRollback(
                        HostRollbackOperation.Company,
                        () => _companyRepository.RemoveCompanyAdminAsync(companyId, userId)
                    )
                );
            }

            if (rollbackEventsPending)
            {
                rollbackTasks.Add(
                    RunRollback(
                        HostRollbackOperation.Events,
                        () => _eventRepository.RemoveAdminFromEvents(companyId, userId)
                    )
                );
            }

            var results = await Task.WhenAll(rollbackTasks);

            foreach (var result in results)
            {
                if (result.Success)
                {
                    switch (result.Operation)
                    {
                        case HostRollbackOperation.Rca:
                            rollbackRcaPending = false;
                            break;
                        case HostRollbackOperation.Company:
                            rollbackCompanyPending = false;
                            break;
                        case HostRollbackOperation.Events:
                            rollbackEventsPending = false;
                            break;
                    }
                }
            }
            
            if (retries++ > 4)
            {
                throw new MongoFailedToUpdateException("rollback");
            }
            if (rollbackRcaPending || rollbackCompanyPending || rollbackEventsPending)
            {
                Console.WriteLine($"retrying, {rollbackRcaPending} {rollbackCompanyPending} {rollbackEventsPending}");
                await Task.Delay(150);
            }
        }
        return false;
        // successfully rolled back
    }

    public async Task<List<RequestCompanyAdminPublic>> GetRcaByHostIdStatusAsync(string hostId, RcaStatus? status)
    {
        var result =  await _requestCompanyAdminRepository.GetByIdStatusAsync(hostId, status);
        return result.ConvertAll(r => r.ToPublic());
    }

    public async Task<RequestCompanyAdminPublic?> GetRcaByIdAsyncProtected(ObjectId id, string userId)
    {
        var result = await _requestCompanyAdminRepository.GetAsyncProtected(id, userId);
        return result?.ToPublic();
    }

    public async Task<bool> DeclineRcaAsync(ObjectId rcaId, string userId)
    {
        var result = await _requestCompanyAdminRepository.UpdateAsyncProtected(rcaId, userId, RcaStatus.Declined);
        return result;
    }
}