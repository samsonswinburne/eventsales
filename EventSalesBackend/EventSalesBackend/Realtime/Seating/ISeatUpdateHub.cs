using System.Threading.Channels;
using EventSalesBackend.Realtime.Seating;
using MongoDB.Bson;

namespace EventSalesBackend.Services.Interfaces;

public interface ISeatUpdateHub
{
    IAsyncEnumerable<SeatUpdate> SubscribeAsync(
        ObjectId eventId,
        CancellationToken cancellationToken = default);
    ValueTask PublishAsync(ObjectId eventId, SeatUpdate update, CancellationToken cancellationToken = default);
}