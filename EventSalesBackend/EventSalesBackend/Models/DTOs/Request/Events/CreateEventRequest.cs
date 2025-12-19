using System.ComponentModel.DataAnnotations;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;

namespace EventSalesBackend.Models.DTOs.Request.Events;

public class CreateEventRequest
    {

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public required string Name { get; set; }

        [Required]
        public required bool InPersonEvent { get; set; }

        public string? VenueAddress { get; set; }

        [Range(0, 100)]
        public int IndividualPurchaseLimit { get; set; } = 0;

        public JsonVenueLocation? VenueLocation { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

public static class CreateEventRequestExtensions
{
    public static Event ToEvent(this CreateEventRequest request)
    {
        throw new NotImplementedException();
    }
}
    