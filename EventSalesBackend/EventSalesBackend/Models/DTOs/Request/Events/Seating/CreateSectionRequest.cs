namespace EventSalesBackend.Models.DTOs.Request.Events.Seating;

public class CreateSectionSeat
{
    public string SeatNumber { get;init; }
    public string? Row { get;init; }
}

public class CreateSectionRequest
{
    public string TicketTypeId  { get; init; }
    public bool IsAssignedSeats { get;init; }
    public int? Capacity { get;init; }
    public List<CreateSectionSeat>? Seats { get;init; }
    public string Name { get;init; }
}

