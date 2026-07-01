using MongoDB.Bson;

namespace EventSalesBackend.Models.DTOs.Data.Seat;

public class SeatHoldIdentifier
{
    public ObjectId EventId;
    public ObjectId SectionId;
    public string SeatNumber;
    public string? Row;
}