using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    public CompanyService(ICompanyRepository repository)
    {
        _companyRepository = repository;
    }
    public async Task<CompanyPublic?> GetPublicAsync(ObjectId id)
    {
        var result = await _companyRepository.GetAsync(id);
        if (result is null)
        {
            return null;
        }

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
        {
            return new CreateCompanyResponse
            {
                CompanyId = company.Id.ToString()
            };
        }

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
}