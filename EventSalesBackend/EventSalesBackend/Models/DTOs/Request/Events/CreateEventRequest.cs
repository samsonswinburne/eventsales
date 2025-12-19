using System.ComponentModel.DataAnnotations;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;

namespace EventSalesBackend.Models.DTOs.Request.Events;

public class CreateEventRequest
    {

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }
        
        [StringLength(100, MinimumLength = 3)]
        public string? Description { get; set; }
        [Required]
        public required bool InPersonEvent { get; set; }

        public string? VenueAddress { get; set; }

        [Range(0, 100)]
        public int IndividualPurchaseLimit { get; set; } = 0;
        

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

public static class CreateEventRequestExtensions
{
    public static Event ToEvent(this CreateEventRequest obj, List<string> admins, CompanySummary summary)
    {
        return new Event
        {
            Name = obj.Name,
            InPersonEvent = obj.InPersonEvent,
            VenueAddress = obj.VenueAddress,
            IndividualPurchaseLimit = obj.IndividualPurchaseLimit,
            StartDate = obj.StartDate,
            EndDate = obj.EndDate,
            Admins = admins ?? throw new ArgumentNullException(nameof(admins)),
            Created = DateTime.UtcNow,
            Description = obj.Description,
            HostCompanySummary = summary,
            TicketTypes = [],
            Summary = new TicketSummary(),
            Photo = "" // default photo string
        };
    }
}
