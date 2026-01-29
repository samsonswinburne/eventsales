namespace EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;

public class TicketStatusResponse
{
    public TicketStatusResponse(TicketStatus ticketStatus)
    {
        Status = ticketStatus;
    }
    public TicketStatus Status { get; init; }
}