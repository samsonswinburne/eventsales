using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Seat;
using MongoDB.Driver;

namespace EventSalesBackend.Repositories.Interfaces;

public interface ISeatHoldRepository
{
    Task<SeatHoldData> Insert(SeatHold seatHold, CancellationToken cancellationToken);
    Task<SeatHoldData> Update(string userId, SeatHoldIdentifier seatHoldIdentifier, UpdateDefinition<SeatHold> update,
        CancellationToken cancellationToken);
    Task<SeatHoldData> Check(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken);
    Task<bool> Remove(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken);

    Task<List<SeatHold>> GetSeatHolds(string userId, CancellationToken cancellationToken);

}