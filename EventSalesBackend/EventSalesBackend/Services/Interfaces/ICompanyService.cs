using Amazon.Runtime.Internal;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using System.ComponentModel.Design;

namespace EventSalesBackend.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyPublic?> GetPublicAsync(ObjectId id);
    Task<bool> UpdateAsync(ObjectId id, Company company);
    Task<CreateCompanyResponse?> CreateAsync(Company company);
    Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId);
    Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId);
    Task<RequestCompanyAdminPublic?> InviteAdminAsync(string userId, ObjectId companyId, string email);
    Task<bool> DeclineAdminRequestAsync(ObjectId rcaId, string userId);
    Task<bool> AcceptAdminRequestAsync(ObjectId rcaId, string userId);
}