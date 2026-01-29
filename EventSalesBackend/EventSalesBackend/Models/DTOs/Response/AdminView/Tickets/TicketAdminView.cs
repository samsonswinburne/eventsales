namespace EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;

public class TicketAdminView
{
    public required string Id { get; init; }
    public required string EventId { get; init; } // not sure if this is needed but I'm keeping it for now - if i was to group viewing tickets into by 
    public required string TicketTypeId { get; init; }
    public string? CustomerId { get; init; }
    public required string CustomerEmail { get; init; } // for now you can view customer emails who have purchased your tickets, i think this will stay
    public required string CustomerName { get; init; }
    public string? CustomerPhone  { get; init; }
    public required DateTime PurchaseTime { get; init; }
    public required bool OrderDelivered { get; init; } // both could be helpful to the event organiser in the event of a bug
    public DateTime? OrderDeliveryDate { get; init; }// both could be helpful to the event organiser in the event of a bug
    public required List<TicketScanJson> ScanHistory { get; init; }
    public required TicketStatus Status { get; init; }
    
} // difference between this and a customer copy would be scanhistory, key, possibly Id

public class TicketScanJson
{
    public required string Id { get; init; }
    public required DateTime ScanTime { get; init; }
    public string? ScannerUserId { get; init; } // not entirely sure what this will look like right now
    public required TicketScanAction Action { get; init; }
}