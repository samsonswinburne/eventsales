using EventSalesBackend.Models;

namespace EventSalesBackend.Repositories.Interfaces
{
    public interface IRequestCompanyAdminRepository
    {
        Task<bool> CreateAsync(RequestCompanyAdmin rca);
    }
}
