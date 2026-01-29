namespace EventSalesBackend.Models.DTOs.Response.PublicInfo
{
    public class TicketPublic
    {
        public required string Id { get; init; }

        public required string EventId { get; init; }
        
        public required string TicketTypeId { get; init; }
        
        public string? CustomerId { get; init; }

        public required string CustomerEmail { get; init; }

        public required string CustomerName { get; init; }

        public string? CustomerPhone { get; init; }

        public required DateTime PurchaseTime { get; init; }

        public bool OrderDelivered { get; init; } = false;

        public DateTime? OrderDeliveryDate { get; init; }
        
        public required string Key { get; init; }
        public required TicketStatus Status { get; init; }
    }

}
