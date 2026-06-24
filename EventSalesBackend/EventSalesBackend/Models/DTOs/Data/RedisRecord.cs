using MongoDB.Bson;

namespace EventSalesBackend.Models.DTOs.Data;

public class RedisRecord
{
    public ObjectId EventId { get; }
    public ObjectId SectionId { get; }
    public string UserId { get; }
    public string SeatNumber { get; }
    public string? Row { get; }
    public RedisRecord(string redisString)
    {
        var members = redisString.Split(":");
        EventId = new ObjectId(members[2]);
        UserId = (members[4]);
        SectionId = new ObjectId(members[6]);
        SeatNumber = (members[7]);
        Row = String.IsNullOrWhiteSpace(redisString) ? null : redisString;
    }
}