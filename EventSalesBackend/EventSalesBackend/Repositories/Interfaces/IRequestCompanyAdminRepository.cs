using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IRequestCompanyAdminRepository
    {
        Task<bool> CreateAsync(RequestCompanyAdmin rca);
        Task<bool> UpdateAsyncProtected(ObjectId rcaId, string responderId, RcaStatus status);
    }
}
