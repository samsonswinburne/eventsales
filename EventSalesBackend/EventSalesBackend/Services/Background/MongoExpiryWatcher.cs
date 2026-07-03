using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Realtime.Seating;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Services.Background;

public class MongoExpiryWatcher : BackgroundService
{
    private const string WatcherName = "MongoExpiryWatcher";
    private readonly IMongoCollection<SeatHold> _seatHolds;
    private readonly IMongoCollection<StreamCheckpoint> _streamCheckpoints;
    private readonly ISeatUpdateHub _seatUpdateHub;
    public MongoExpiryWatcher(IMongoDbContext mongoDbContext, ISeatUpdateHub hub)
    {
        _seatHolds = mongoDbContext.SeatHolds;
        _streamCheckpoints = mongoDbContext.StreamCheckpoints;
        _seatUpdateHub = hub;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var checkpoint = await _streamCheckpoints
            .Find(c => c.WatcherName == WatcherName && c.InstanceId == AppIdentity.InstanceId)
            .FirstOrDefaultAsync(stoppingToken);
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<SeatHold>>()
            .Match(x => x.OperationType == ChangeStreamOperationType.Delete);
        var options = new ChangeStreamOptions
        {
            FullDocumentBeforeChange = ChangeStreamFullDocumentBeforeChangeOption.WhenAvailable,
            ResumeAfter = checkpoint?.ResumeToken,
        };
        
        using var cursor = await _seatHolds.WatchAsync(pipeline, options, stoppingToken);
        await cursor.ForEachAsync(async change =>
        {
            var seatHold = change.FullDocumentBeforeChange;
            
            Console.WriteLine(seatHold);
            _seatUpdateHub.PublishAsync(seatHold.EventId, new SeatUpdate
            {
                SeatNumber = seatHold.SeatNumber,
                SectionId =  seatHold.SectionId,
                Row = seatHold.Row,
                Status = SeatHoldStatus.Expired
            });
            
            
            await _streamCheckpoints.ReplaceOneAsync(
                c => c.WatcherName == WatcherName,
                new StreamCheckpoint { WatcherName = WatcherName, ResumeToken = change.ResumeToken, InstanceId = AppIdentity.InstanceId},
                new ReplaceOptions { IsUpsert = true },
                stoppingToken);
        }, stoppingToken);
        
    }
}