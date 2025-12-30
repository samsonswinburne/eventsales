using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace EventSalesBackend.Models.DTOs.Response.PublicInfo;

// not completed, not yet sure if anything needs to be removed from the event
public class EventPublic
{
    public required string Id { get; set; }


    public required CompanySummary HostCompanySummary { get; set; }


    public required string Name { get; set; }


    public string? Description { get; set; }


    public required List<TicketTypePublic> TicketTypes { get; set; } = new();


    public required TicketSummary Summary { get; set; }


    public required string Photo { get; set; }


    public string? PostCode { get; set; } // if its an online event then not needed


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
    [Required] [Range(-180, 180)] public required double Latitude { get; set; }

    [Required] [Range(-90, 90)] public required double Longitude { get; set; }
}

public class TicketTypePublic
{

    public required string Id { get; set; }
    public required ObjectId TestId { get; init; }

    public required string Name { get; set; }


    public required string Description { get; set; } = string.Empty;


    public required int TotalAvaliable { get; set; }

    public required int Sold { get; set; } = 0;
    
    
    public required decimal Price { get; set; }
    
    public decimal? DiscountedPrice { get; set; }

}