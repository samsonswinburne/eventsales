using EventSalesBackend.Models;
using MongoDB.Bson;

namespace EventSalesBackend.Realtime.Seating;


public class SeatUpdate
{
    public ObjectId SectionId {get; set;}
    public string SeatNumber  {get; set;}
    public string? Row {get; set;}
    
    public SeatHoldStatus Status {get; set;}
}
public class SeatUpdateJsonFormat
{
    public string SectionId {get; set;}
    public string SeatNumber  {get; set;}
    public string? Row {get; set;}
    
    public SeatHoldStatus Status {get; set;}
}

public static class SeatUpdateExtensions
{
    public static SeatUpdateJsonFormat ToJsonFormat(this SeatUpdate update)
    {
        return new SeatUpdateJsonFormat
        {
            SectionId = update.SectionId.ToString(),
            SeatNumber = update.SeatNumber.ToString(),
            Row = update.Row?.ToString(),
            Status = update.Status
        };
    }
}