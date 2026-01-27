using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetAsync(ObjectId id);
    Task<bool> UpdateAsync(ObjectId id, Company company);
    Task<ObjectId> CreateAsync(Company company);
    Task<bool> AddCompanyAdminAsync(ObjectId companyId, string adminId);
    Task<bool> RemoveCompanyAdminAsync(ObjectId companyId, string userId);
    Task<bool> RemoveCompanyAdminProtectedAsync(ObjectId companyId, string ownerId, string userId);
    Task<bool> UpdateOwnerIdProtectedAsync(ObjectId companyId, string requestingUserId, string setOwnerUserId);
    Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId);

    Task<List<CompanySummary>?> GetCompanySummariesByUserId(string userId);
}