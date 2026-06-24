using EventSalesBackend.Services.Interfaces;
using Medallion.Threading;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class SeatLockService : ISeatLockService
{
    private readonly IDistributedLockProvider _lockProvider;
    public SeatLockService(IDistributedLockProvider distributedLockProvider)
    {
        _lockProvider = distributedLockProvider;
    }
    
    public async Task<bool> AcquireLockAsync(ObjectId eventId, ObjectId sectionId, string userId, string seatNumber, string? row,
        CancellationToken cancellationToken, int durationSeconds=10)
    {
        var key = $"lock:event:{eventId.ToString()}:user:{userId}:seat:{sectionId.ToString()}:{seatNumber}:{row}";
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMilliseconds(300)); // max wait time
        await using var handle = await _lockProvider.TryAcquireLockAsync(
            key,
            TimeSpan.FromSeconds(durationSeconds), // TTL (how long lock lives)
            cts.Token);       // cancel acquisition if needed
        
        return handle != null;
    }



    public async Task<bool> ReleaseLockAsync(ObjectId eventId, ObjectId sectionId, string userId, string seatNumber, string? row,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}