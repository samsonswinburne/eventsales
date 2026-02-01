namespace EventSalesBackend.Models.DTOs.Request.Tickets;

public class UpdateTicketStatusRequest
{
    public int Status { get; init; }
    public bool Override { get; init; }
}