using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models;



public class SeatHold
{
    [BsonElement("eventId")]
    public required ObjectId EventId { get; set; }
    [BsonElement("sectionId")]
    public required ObjectId SectionId { get; set; }
    [BsonElement("ticketTypeId")]
    [BsonRequired]
    public required ObjectId TicketTypeId { get; set; }
    [BsonElement("userId")]
    public required string UserId { get; set; }
    [BsonElement("seatNumber")]
    public required string SeatNumber { get; set; }
    [BsonElement("row")]
    public required string? Row { get; set; }
    [BsonElement("expiresAt")]
    public required DateTime ExpiresAt { get; set; }
}

public enum SeatHoldStatus
{
    Expired,
    Active,
    InCheckout,
}