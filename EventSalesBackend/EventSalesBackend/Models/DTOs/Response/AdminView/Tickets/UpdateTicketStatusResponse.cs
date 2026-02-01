namespace EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;

public class UpdateTicketStatusResponse
{
    public required TicketStatus Status { get; init; }
}