using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Repositories.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        public Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectId> CreateAsync(Company company)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ObjectId id, Company company)
        {
            throw new NotImplementedException();
        }
    }
}
