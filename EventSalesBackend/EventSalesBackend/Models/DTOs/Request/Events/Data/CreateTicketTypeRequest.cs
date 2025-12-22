using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Request.Events.Data;

public class CreateTicketTypeRequest
{
    [Required]
    public required string EventId {get; init;}
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string TicketName {get; init;}
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Description {get; init;}
    [Required]
    [Range(0, 99999)]
    public required int TotalAvailable {get; init;}
    [Required]
    [Range(0, 99999)]
    public required int Sold  {get; init;}
    [Required]
    public required bool Enabled {get; init;}
    [Required]
    [Range(0, 9999)]
    public required decimal Price {get; init;}
}

public static class CreateTicketTypeRequestExtensions
{
    public static TicketType ToTicketType(this CreateTicketTypeRequest request)
    {
        return new TicketType
        {
            Description = request.Description,
            DiscountedPrice = null,
            Enabled = request.Enabled,
            Name = request.TicketName,
            Price = request.Price,
            Sold = request.Sold,
            TotalAvaliable = request.TotalAvailable
        };
    }
}