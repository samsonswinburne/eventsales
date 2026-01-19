using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Response.PublicInfo;

// not completed, not yet sure if anything needs to be removed from the event
public class EventPublic
{
    public required string Id { get; set; }


    public required CompanySummaryJson HostCompanySummary { get; init; }


    public required string Name { get; init; }


    public string? Description { get; init; }


    public required List<TicketTypePublic> TicketTypes { get; init; } = new();


    public required TicketSummary Summary { get; init; }


    public required string Photo { get; init; }


    public string? PostCode { get; init; } // if its an online event then not needed


    public required bool InPersonEvent { get; init; }


    public string? VenueAddress { get; init; }


    public required int IndividualPurchaseLimit { get; init; } = 0;

    public JsonVenueLocation? VenueLocation { get; init; }


    public required EventStatus Status { get; set; }


    public required DateTime StartDate { get; init; }


    public required DateTime EndDate { get; init; }
}

public class JsonVenueLocation
{
    [Required] [Range(-180, 180)] public required double Latitude { get; set; }

    [Required] [Range(-90, 90)] public required double Longitude { get; set; }
}

public class TicketTypePublic
{

    public required string Id { get; init; }

    public required string Name { get; init; }


    public required string Description { get; init; } = string.Empty;


    public required int TotalAvaliable { get; init; }

    public required int Sold { get; init; } = 0;
    
    
    public required decimal Price { get; init; }
    
    public decimal? DiscountedPrice { get; init; }

}
public class CompanySummaryJson
{
    public required string CompanyId { get; init; }

    public required string CompanyName { get; init; }

    public required string CompanyImageUrl { get; init; }
}