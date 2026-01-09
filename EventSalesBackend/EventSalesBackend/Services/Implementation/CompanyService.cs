using EventSalesBackend.Exceptions.Companies;
using EventSalesBackend.Exceptions.Companies.DTOs;
using EventSalesBackend.Exceptions.Hosts;
using EventSalesBackend.Exceptions.MongoDB;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IRequestCompanyAdminRepository _requestCompanyAdminRepository;
    private readonly IHostRepository _hostRepository;
    private readonly IEventRepository _eventRepository;
    

    public CompanyService(ICompanyRepository repository, IHostRepository hostService, IRequestCompanyAdminRepository requestCompanyAdminRepository, IEventRepository eventRepository)
    {
        _companyRepository = repository;
        _hostRepository = hostService;
        _requestCompanyAdminRepository = requestCompanyAdminRepository;
        _eventRepository = eventRepository;
    }

    public async Task<CompanyPublic?> GetPublicAsync(ObjectId id)
    {
        var result = await _companyRepository.GetAsync(id);
        if (result is null) return null;

        return result.ToPublic();
    }

    public async Task<bool> UpdateAsync(ObjectId id, Company company)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateCompanyResponse?> CreateAsync(Company company)
    {
        var host = await _hostRepository.GetAsync(company.OwnerId);
        if(host?.Id is null)
        {
            throw new HostNotFoundException(null, company.OwnerId);
        }
        await _companyRepository.CreateAsync(company);
        if (company.Id != default)
            return new CreateCompanyResponse
            {
                CompanyId = company.Id.ToString()
            };

        return null;
    }

    public async Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId)
    {
        return await _companyRepository.AddCompanyAdminAsync(companyId, adminId);
    }

    public async Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId)
    {
        return await _companyRepository.GetAdminSummaryAsync(companyId, userId);
    }

    public async Task<RequestCompanyAdminPublic?> InviteAdminAsync(string userId, ObjectId companyId, string email)
    {
        
        var adminSummaryTask = _companyRepository.GetAdminSummaryAsync(companyId, userId);
        var userToInviteTask = _hostRepository.GetByEmailAsync(email);

        await Task.WhenAll(adminSummaryTask, userToInviteTask);
        var adminSummary = await adminSummaryTask;
        var userToInvite = await userToInviteTask;

        if(userToInvite?.Id is null)
        {
            throw new HostNotFoundException(email);
        }
        if (!userToInvite.OnBoardingCompleted)
        {
            throw new HostNotCompletedOnboardingException(userToInvite.Id);
        }

        if (adminSummary?.Admins is null)
        {
            throw new AdminSummaryNotFoundException(companyId); 
        }
        if (!adminSummary.Value.Admins.Contains(userId))
        {
            throw new UserNotAdminException(userId, companyId);
        }

        var rca = new RequestCompanyAdmin
        {
            CompanyId = companyId,
            RequestSenderId = userId,
            RequestReceiverId = userToInvite.Id,
            Status = RcaStatus.Pending,
            SentTime = DateTime.UtcNow
        };
        // write rca to database
        await _requestCompanyAdminRepository.CreateAsync(rca);
        if (rca.Id == ObjectId.Empty) throw new MongoInsertException("requestCompanyAdmin");
        
        return rca.ToPublic();
    }

    public async Task<bool> DeclineAdminRequestAsync(ObjectId rcaId, string userId)
    {
        var updateRcaResult
            = await _requestCompanyAdminRepository.UpdateAsyncProtected(rcaId, userId, RcaStatus.Declined);
        if (!updateRcaResult)
        {
            throw new MongoFailedToUpdateException("requestCompanyAdmin");
        }

        return true;
        
    }

    public async Task<bool> CancelAdminRequestAsync(ObjectId rcaId, string userId)
    {
        var updateRcaResult =
            await _requestCompanyAdminRepository.UpdateAsyncProtected(rcaId, userId, RcaStatus.Cancelled);
        if (!updateRcaResult)
        {
            throw new MongoFailedToUpdateException("requestCompanyAdmin");
        }
        return true;
    }

    public async Task<bool> AddAdminAsync(ObjectId companyId, string userId)
    {
        var result = await _companyRepository.AddCompanyAdminAsync(companyId, userId);
        return result;
    }

    public async Task<bool> RemoveAdminAsync(ObjectId companyId, string userId)
    {
        var result = await _companyRepository.RemoveCompanyAdminAsync(companyId, userId);
        return result;
    }
    public async Task<bool> RemoveAdminProtectedAsync(ObjectId companyId, string ownerId, string userId) // will be called from company controller
    {
        var removeFromCompany = await _companyRepository.RemoveCompanyAdminProtectedAsync(companyId, ownerId, userId);

        if (!removeFromCompany) throw new MongoFailedToUpdateException("company");
        // either unauthorised or admin does not exist at company

        // if the first request is successful then we can guarantee that the requester is company owner
        var removeFromEvents = await _eventRepository.RemoveAdminFromEvents(companyId, userId);
        if (!removeFromEvents)
        {
            // removing from events has failed, now we should roll back removing from company
            var rollbackPending = true;
            var retries = 0;

            while (rollbackPending)
            {
                rollbackPending = !await _companyRepository.AddCompanyAdminAsync(companyId, userId);

                if (retries++ > 4)
                {
                    throw new MongoFailedToUpdateException("rollback company");
                }
                if (rollbackPending) Thread.Sleep(1500);
            }
            if (!removeFromCompany) throw new MongoFailedToUpdateException("company");
        }
        return true;
    }
}