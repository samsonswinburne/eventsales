using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyPublic?> GetPublicAsync(ObjectId id);
    Task<bool> UpdateAsync(ObjectId id, Company company);
    Task<CreateCompanyResponse?> CreateAsync(Company company);
    Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId, List<string>? adminIds);
    Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId);
}