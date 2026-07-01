using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models;

public class Seat
{
    [BsonElement("sectionId")]
    [BsonRequired]
    public required ObjectId SectionId { get; set; }
    [BsonElement("eventId")]
    [BsonRequired]
    public required ObjectId EventId { get; set; }
    [BsonRequired]
    [BsonElement("ticketTypeId")]
    public required ObjectId TicketTypeId { get; set; }
    [BsonElement("number")]
    public required string SeatNumber { get; set; }
    [BsonElement("row")]
    public string? Row { get; set; }
}
public class Section
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("type")]
    public required SectionType Type { get; set; }
    [BsonElement("seats")]
    public required List<Seat> Seats { get; set; }
    [BsonElement("capacity")]
    public int? Capacity { get; set; }
    [BsonElement("ticketTypeId")][BsonRequired] public required ObjectId TicketTypeId { get; set; }
}

public class SeatJsonFormat
{

    public required string SectionId { get; init; }
    public required string EventId { get; init; }
    public required string TicketTypeId { get; init; }
    public required string SeatNumber { get; init; }
    public required string? Row { get; init; }
}
public class SectionJsonFormat
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required SectionType Type { get; init; }
    public required List<SeatJsonFormat> Seats { get; init; }
    public int? Capacity { get; init; }
    public required string TicketTypeId { get; init; }
}

public static class SeatExtensions
{
    public static SeatJsonFormat ToJsonFormat(this Seat seat)
    {
        return new SeatJsonFormat
        {
            SectionId = seat.SectionId.ToString(),
            EventId = seat.EventId.ToString(),
            TicketTypeId = seat.TicketTypeId.ToString(),
            SeatNumber = seat.SeatNumber,
            Row = seat.Row,
        };
    }
}

public static class SectionExtensions
{
    public static SectionJsonFormat ToJsonFormat(this Section section)
    {
        return new SectionJsonFormat
        {
            Id = section.Id.ToString(),
            Name = section.Name,
            Type = section.Type,
            Seats = section.Seats.ConvertAll(s => s.ToJsonFormat()),
            Capacity = section.Capacity,
            TicketTypeId = section.TicketTypeId.ToString(),
        };
    }
}