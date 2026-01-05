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

        public async Task<RequestCompanyAdmin?> GetAsyncProtected(ObjectId rcaId, string userId)
        {
            // currently allows the request reciever and sender to view it. In the future it should be the admins can view it as well
            var filter = Builders<RequestCompanyAdmin>.Filter.And(
                Builders<RequestCompanyAdmin>.Filter.Eq(r => r.Id, rcaId),
                Builders<RequestCompanyAdmin>.Filter.Or(
                    Builders<RequestCompanyAdmin>.Filter.Eq(r => r.RequestReceiverId, userId),
                    Builders<RequestCompanyAdmin>.Filter.Eq(r => r.RequestSenderId, userId)
                    )
                );
            return await _rcas.Find(filter).FirstOrDefaultAsync();

        }

        public async Task<List<RequestCompanyAdmin>> GetByIdStatusAsync(string responderId, RcaStatus? status)
        {
            var idFilter = 
                Builders<RequestCompanyAdmin>.Filter.Eq(r => r.RequestReceiverId, responderId);

            var statusFilter = Builders<RequestCompanyAdmin>.Filter.Empty;

            if (status != null)
            {
                statusFilter = Builders<RequestCompanyAdmin>.Filter.Eq(r => r.Status, status);
            }

            var idStatusFilter = Builders<RequestCompanyAdmin>.Filter.And(idFilter, statusFilter);

            return await _rcas.Find(idStatusFilter).ToListAsync();

        }

        public async Task<bool> UpdateAsyncProtected(ObjectId rcaId, string responderId, RcaStatus status)
        {
            var rcaIdresponderIdFilter = Builders<RequestCompanyAdmin>.Filter.And(
                Builders<RequestCompanyAdmin>.Filter.Eq(r => r.Id, rcaId),
                Builders<RequestCompanyAdmin>.Filter.Eq(r => r.RequestReceiverId, responderId),
                Builders<RequestCompanyAdmin>.Filter.Eq(r => r.Status, RcaStatus.Pending) // only pending requests can be approved / denied / canceled
                );
            var update = Builders<RequestCompanyAdmin>.Update.
                Set(r => r.Status, status)
                .Set(r => r.UpdatedTime, DateTime.UtcNow);

            var result = await _rcas.UpdateOneAsync(rcaIdresponderIdFilter, update);
            return result.ModifiedCount > 0;
        }
    }
}
