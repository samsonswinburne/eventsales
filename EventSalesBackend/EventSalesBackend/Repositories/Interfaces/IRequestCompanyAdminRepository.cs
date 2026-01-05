using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IRequestCompanyAdminRepository
    {
        Task<bool> CreateAsync(RequestCompanyAdmin rca);
        Task<bool> UpdateAsyncProtected(ObjectId rcaId, string responderId, RcaStatus status);
        Task<RequestCompanyAdmin?> GetAsyncProtected(ObjectId rcaId, string responderId);
        Task<List<RequestCompanyAdmin>> GetByIdStatusAsync(string responderId, RcaStatus? status);
    }
}
