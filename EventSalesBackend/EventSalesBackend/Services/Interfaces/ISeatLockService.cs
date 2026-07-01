using EventSalesBackend.Models.DTOs.Data.Seat;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface ISeatLockService
{
    Task<SeatHoldData> AcquireLockAsync(string userId, SeatHoldIdentifier seatHoldIdentifier,
        CancellationToken cancellationToken,
        int durationSeconds =
            10); // could potentially add refresh Locks to refresh all locks currently belonging to one person
    // and could potentially add a type for this because as of right now they all have the same parameters
}