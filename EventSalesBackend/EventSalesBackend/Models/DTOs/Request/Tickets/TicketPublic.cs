namespace EventSalesBackend.Models.DTOs.Request.Tickets
{
    public class TicketPublic
    {
        public required string Id { get; init; }

        public required string EventId { get; init; }

        public string? CustomerId { get; init; }

        public required string CustomerEmail { get; init; }

        public required string CustomerName { get; init; }

        public string? CustomerPhone { get; init; }

        public required DateTime PurchaseTime { get; init; }

        public bool OrderDelivered { get; init; } = false;

        public DateTime? OrderDeliveryDate { get; init; }
    }

}
