using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation
{
    public class RequestCompanyAdminRepository : IRequestCompanyAdminRepository
    {
        private readonly IMongoCollection<RequestCompanyAdmin> _rcas;
        public RequestCompanyAdminRepository(IMongoDbContext mongoDbContext)
        {
            _rcas = mongoDbContext.CompanyAdminRequests;
        }
        public async Task<bool> CreateAsync(RequestCompanyAdmin rca)
        {
            await _rcas.InsertOneAsync(rca);
            if(rca.Id == null)
            {
                return false;
            }
            return true;
        }
        public async Task<RequestCompanyAdmin?> GetAsync(ObjectId rcaId)
        {
            var filter = Builders<RequestCompanyAdmin>.Filter.Eq(r => r.Id, rcaId);
            return await _rcas.Find(filter).FirstOrDefaultAsync();
        }
    }
}
