using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Seat;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Interfaces;

public interface ISeatHoldService
{
    Task<SeatHoldData> Create(string userId, SeatHoldIdentifier seatHoldIdentifier, CancellationToken cancellationToken);
    Task<List<SeatHold>> FindByUserId(string userId, CancellationToken cancellationToken);
    Task<bool> UpdateManyByUserIdAsync(string userId,UpdateDefinition<SeatHold> update, IClientSessionHandle session,CancellationToken cancellationToken);

    Task<bool> Remove(string userId, SeatHoldIdentifier seatHoldIdentifier,
        CancellationToken cancellationToken);
}