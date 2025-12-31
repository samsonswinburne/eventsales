using EventSalesBackend.Data;
using EventSalesBackend.Exceptions.Companies;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class CompanyRepository : ICompanyRepository
{
    private readonly IMongoCollection<Company> _companyRepository;

    public CompanyRepository(IMongoDbContext context)
    {
        _companyRepository = context.Companies;
    }

    public async Task<bool> AddCompanyAdmin(ObjectId companyId, string adminId, List<string>? adminIds)
    {
        var update = Builders<Company>.Update.AddToSet(c => c.Admins, adminId);

        if (adminIds is not null)
        {
            adminIds.Add(adminId); // need to add dup check
            update = Builders<Company>.Update.Set(c => c.Admins, adminIds);
        }

        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var result = await _companyRepository.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }


    public async Task<AdminSummaryDTO?> GetAdminSummaryAsync(ObjectId companyId, string userId)
    {
        /*var projection = Builders<Company>.Projection
            .Include(c => c.Admins)
            .Include(c => c.Name)
            .Include(c => c.Id)
            .Include(c => c.LogoUrl);*/

        var projection = Builders<Company>.Projection.Expression(x => new AdminSummaryDTO
            {
                Admins = x.Admins,
                Summary = new CompanySummary
                {
                    CompanyId = x.Id,
                    CompanyName = x.Name,
                    CompanyImageUrl = x.LogoUrl
                }
            }
        );

        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);

        AdminSummaryDTO? result = await _companyRepository.Find(filter).Project(projection).FirstOrDefaultAsync();
        if (result is null)
        {
            throw new CompanyNotFoundException(companyId.ToString());
        }
        return !result.Value.Admins.Contains(userId) ? throw new UnauthorizedAccessException() : result;
    }


    public async Task<ObjectId> CreateAsync(Company company)
    {
        await _companyRepository.InsertOneAsync(company);
        return company.Id;
    }

    public async Task<Company?> GetAsync(ObjectId id)
    {
        return await _companyRepository.Find(company => company.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(ObjectId id, Company company)
    {
        var result = await _companyRepository.ReplaceOneAsync(c => c.Id == id, company);
        return result.ModifiedCount > 0;
    }
    
}