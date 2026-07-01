using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Seat;
using EventSalesBackend.Repositories.Interfaces;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Implementation;

public class SeatHoldRepository : ISeatHoldRepository
{
    private readonly IMongoCollection<SeatHold> _seatHolds;
    private readonly IMongoDbContext _dbContext;

    public SeatHoldRepository(IMongoDbContext context)
    {
        _seatHolds = context.SeatHolds;
        _dbContext = context;
    }
    

    public async Task<SeatHoldData> Insert(SeatHold seatHold, CancellationToken cancellationToken)
    {
        try
        {
            await _seatHolds.InsertOneAsync(seatHold, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException)
        {
            return new SeatHoldData { Acquired = false };
        }

        return new SeatHoldData
        {
            Acquired = true,
            ExpiresAt = seatHold.ExpiresAt,
        };
    }

    public async Task<SeatHoldData> Update(string userId, SeatHoldIdentifier seatHoldIdentifier, UpdateDefinition<SeatHold> update, CancellationToken cancellationToken)
    {
        var filter = Builders<SeatHold>.Filter.And(
            Builders<SeatHold>.Filter.Eq(s => s.Row, seatHoldIdentifier.Row),
            Builders<SeatHold>.Filter.Eq(s => s.SeatNumber, seatHoldIdentifier.SeatNumber),
            Builders<SeatHold>.Filter.Eq(s => s.SectionId, seatHoldIdentifier.SectionId),
            Builders<SeatHold>.Filter.Eq(s => s.EventId, seatHoldIdentifier.EventId),
            Builders<SeatHold>.Filter.Eq(s=>s.UserId, userId)
            );
        
        var result = await _seatHolds.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return new SeatHoldData
        {
            Acquired = result.ModifiedCount > 0 ?  true : false,
        };
    }



    public async Task<SeatHoldData> Check(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken)
    {
        var filter = Builders<SeatHold>.Filter.And(
            Builders<SeatHold>.Filter.Eq(s => s.Row, seatHoldIdentifier.Row),
            Builders<SeatHold>.Filter.Eq(s => s.SeatNumber, seatHoldIdentifier.SeatNumber),
            Builders<SeatHold>.Filter.Eq(s => s.SectionId, seatHoldIdentifier.SectionId),
            Builders<SeatHold>.Filter.Eq(s => s.EventId, seatHoldIdentifier.EventId),
            Builders<SeatHold>.Filter.Eq(s=>s.UserId, userId)
        );
        var result = await _seatHolds.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return new SeatHoldData
            {
                Acquired = false
            };
        }

        return new SeatHoldData
        {
            Acquired = DateTime.UtcNow > result.ExpiresAt,
            ExpiresAt = result.ExpiresAt,
        };
    }

    public async Task<bool> Remove(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken)
    {
        var filter = Builders<SeatHold>.Filter.And(
            Builders<SeatHold>.Filter.Eq(s => s.Row, seatHoldIdentifier.Row),
            Builders<SeatHold>.Filter.Eq(s => s.SeatNumber, seatHoldIdentifier.SeatNumber),
            Builders<SeatHold>.Filter.Eq(s => s.SectionId, seatHoldIdentifier.SectionId),
            Builders<SeatHold>.Filter.Eq(s => s.EventId, seatHoldIdentifier.EventId),
            Builders<SeatHold>.Filter.Eq(s=>s.UserId, userId)
        );
        var result = await _seatHolds.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<SeatHold>> GetSeatHolds(string userId, CancellationToken cancellationToken)
    {
        var filter = Builders<SeatHold>.Filter.Eq(sh => sh.UserId, userId);
        var result = await _seatHolds.Find(filter).ToListAsync(cancellationToken);
        return result;
    }
}