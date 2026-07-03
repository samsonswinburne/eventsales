using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using EventSalesBackend.Realtime.Seating;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Implementation;

public class SeatUpdateHub : ISeatUpdateHub
{
    private readonly ConcurrentDictionary<ObjectId, ConcurrentDictionary<ObjectId, SeatSubscription>> _events;

    public SeatUpdateHub()
    {
        _events = new ConcurrentDictionary<ObjectId, ConcurrentDictionary<ObjectId, SeatSubscription>>();
    }
    
    public async IAsyncEnumerable<SeatUpdate> SubscribeAsync(ObjectId eventId, 
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var subscriptionId = ObjectId.GenerateNewId();
        var subscription = new SeatSubscription();
        
        var eventSubs = _events.GetOrAdd(
            eventId,
            _ => new ConcurrentDictionary<ObjectId, SeatSubscription>());
        eventSubs[subscriptionId] = subscription;

        try
        {
            await foreach (var update in subscription.Channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return update;
            }
        }
        finally
        {
            eventSubs.TryRemove(subscriptionId, out _);
            subscription.Channel.Writer.Complete();
            if (eventSubs.IsEmpty)
            {
                _events.TryRemove(eventId, out _);
            }
        }
    }
    

    public ValueTask PublishAsync(ObjectId eventId, SeatUpdate update, CancellationToken ct = default)
    {
        if (!_events.TryGetValue(eventId, out var subs))
            return ValueTask.CompletedTask;

        foreach (var sub in subs.Values)
        {
            sub.Channel.Writer.TryWrite(update);
        }

        return ValueTask.CompletedTask;
    }
}