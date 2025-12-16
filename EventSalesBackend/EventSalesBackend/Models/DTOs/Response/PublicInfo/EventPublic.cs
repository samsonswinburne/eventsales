using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Response.PublicInfo
{
    // not completed, not yet sure if anything needs to be removed from the event
    public class EventPublic
    {

        public required string Id { get; set; }


        public required CompanySummary HostCompanySummary { get; set; }


        public required string Name { get; set; }


        public required string Description { get; set; }


        public required List<TicketType> TicketTypes { get; set; } = new List<TicketType>();


        public required TicketSummary Summary { get; set; }


        public required string Photo { get; set; }


        public required string PostCode { get; set; }


        public required bool InPersonEvent { get; set; }


        public string? VenueAddress { get; set; }


        public required int IndividualPurchaseLimit { get; set; } = 0;
        
        public JsonVenueLocation? VenueLocation { get; set; }


        public EventStatus Status { get; set; } = EventStatus.Draft;


        public required DateTime StartDate { get; set; }


        public required DateTime EndDate { get; set; }


    }

    public class JsonVenueLocation
    {
        [Required]
        [Range(-180, 180)]
        public required double Latitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public required double Longitude { get; set; }
    }
}
