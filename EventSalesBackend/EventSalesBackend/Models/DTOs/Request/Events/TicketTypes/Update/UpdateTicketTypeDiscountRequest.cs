namespace EventSalesBackend.Models.DTOs.Request.Events.TicketTypes.Update;

public class UpdateTicketTypeDiscountRequest
{
    public decimal Price { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.UtcNow;
    public int? MaximumQuantity {get; set;}
}