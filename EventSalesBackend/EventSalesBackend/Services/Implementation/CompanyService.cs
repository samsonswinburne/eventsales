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
    private readonly IHostService _hostService;
    

    public CompanyService(ICompanyRepository repository, IHostService hostService, IRequestCompanyAdminRepository requestCompanyAdminRepository)
    {
        _companyRepository = repository;
        _hostService = hostService;
        _requestCompanyAdminRepository = requestCompanyAdminRepository;
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
        return await _companyRepository.AddCompanyAdmin(companyId, adminId);
    }

    public async Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId)
    {
        return await _companyRepository.GetAdminSummaryAsync(companyId, userId);
    }

    public async Task<RequestCompanyAdminPublic?> InviteAdminAsync(string userId, ObjectId companyId, string email)
    {
        
        var adminSummaryTask = _companyRepository.GetAdminSummaryAsync(companyId, userId);
        var userToInviteTask = _hostService.GetByEmailAsync(email);

        await Task.WhenAll(adminSummaryTask, userToInviteTask);
        var adminSummary = await adminSummaryTask;
        var userToInvite = await userToInviteTask;

        if(userToInviteTask is null || userToInvite?.Id is null)
        {
            throw new HostNotFoundException(email);
        }

        if (adminSummary is null || adminSummary?.Admins is null)
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
        if (rca.Id is null) throw new MongoInsertException("requestCompanyAdmin");
        
        return rca.ToPublic();
    }

    public Task<RequestCompanyAdminPublic?> InviteAdminAsync(ObjectId userId, ObjectId companyId, string email)
    {
        throw new NotImplementedException();
    }
}