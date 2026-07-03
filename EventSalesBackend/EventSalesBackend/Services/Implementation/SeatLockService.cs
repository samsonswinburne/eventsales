using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Data.Seat;
using EventSalesBackend.Realtime.Seating;
using EventSalesBackend.Services.Interfaces;
using Medallion.Threading;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class SeatLockService : ISeatLockService
{
    private readonly IDistributedLockProvider _lockProvider;
    private readonly ISeatHoldService _seatHoldService;
    private readonly ISeatUpdateHub _seatUpdateHub;
    public SeatLockService(IDistributedLockProvider distributedLockProvider, ISeatHoldService seatHoldService, ISeatUpdateHub seatUpdateHub)
    {
        _lockProvider = distributedLockProvider;
        _seatHoldService = seatHoldService;
        _seatUpdateHub = seatUpdateHub;
    }
    
    public async Task<SeatHoldData> AcquireLockAsync(string userId, SeatHoldIdentifier seatHoldIdentifier,
        CancellationToken cancellationToken, int durationSeconds=10)
    {
        var key = $"lock:event:{seatHoldIdentifier.EventId.ToString()}:seat:{seatHoldIdentifier.SectionId.ToString()}:{seatHoldIdentifier.SeatNumber}:{seatHoldIdentifier.Row}";
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMilliseconds(300)); // max wait time
        await using var handle = await _lockProvider.TryAcquireLockAsync(
            key,
            TimeSpan.FromSeconds(durationSeconds), // TTL (how long lock lives)
            cts.Token);       // cancel acquisition if needed
        if (handle is not null)
        {
            var result = await _seatHoldService.Create(userId, seatHoldIdentifier, cancellationToken);
            // may need a pipeline, also i should really be using value tasks for other stuff in this
             await _seatUpdateHub.PublishAsync(seatHoldIdentifier.EventId,
                new SeatUpdate
                {
                    SectionId = seatHoldIdentifier.SectionId,
                    Row = seatHoldIdentifier.Row,
                    SeatNumber = seatHoldIdentifier.SeatNumber,
                    Status = SeatHoldStatus.Active
                }, cancellationToken
            );
            return result;
        }
        return new SeatHoldData
        {
            Acquired = false
        };
    }



    public async Task<bool> ReleaseLockAsync(ObjectId eventId, ObjectId sectionId, string userId, string seatNumber, string? row,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}