using EventSalesBackend.Exceptions.Hosts;
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
    private readonly IHostService _hostService;

    public CompanyService(ICompanyRepository repository, IHostService hostService)
    {
        _companyRepository = repository;
        _hostService = hostService;
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

    public async Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId, List<string>? adminIds)
    {
        throw new NotImplementedException();
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
            throw new HostNotFoundError(email);
        }
        if (!adminSummary.Value.Admins.Contains(userId))
        {
            throw new UnauthorizedAccessException();
        }

        var rca = new RequestCompanyAdmin
        {
            CompanyId = companyId,
            RequestSenderId = userId,
            RequestReceiverId = userToInvite.Id
        };
        // write rca to database
        throw new NotImplementedException("RCA IS NOT WRITTEN TO DATABASE BECAUSE NO METHOD HAS BEEN WRITTEN YET");
    }

    public Task<RequestCompanyAdminPublic?> InviteAdminAsync(ObjectId userId, ObjectId companyId, string email)
    {
        throw new NotImplementedException();
    }
}