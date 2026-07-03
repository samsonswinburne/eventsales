using System.Threading.Channels;

namespace EventSalesBackend.Realtime.Seating;

public class SeatSubscription
{
    public Channel<SeatUpdate> Channel { get; } =
        System.Threading.Channels.Channel.CreateUnbounded<SeatUpdate>();
}