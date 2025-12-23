using System.ComponentModel.DataAnnotations;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models.DTOs.Response.AdminView.Event;

[BsonIgnoreExtraElements]
public class
    EventAdminView // event should be split up into in person event and digital event, in person event requires different things than digital event
{
    public required string Id { get; set; }


    public required CompanySummaryAdminView HostCompanySummary { get; set; }


    public required string Name { get; set; }


    public string? Description { get; set; }


    public required List<TicketTypeAdminView> TicketTypes { get; set; } = new();


    public required TicketSummary Summary { get; set; }


    public required string Photo { get; set; }

    public string? PostCode { get; set; }

    public required bool InPersonEvent { get; set; }

    public string? VenueAddress { get; set; }

    public int IndividualPurchaseLimit { get; set; } = 0;

    public JsonVenueLocation? VenueLocation { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Draft;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public required List<string> Admins { get; set; }
}

public class TicketTypeAdminView
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("name")]
    [BsonRequired]
    [StringLength(100, MinimumLength = 1)]
    public required string Name { get; set; }

    [StringLength(100, MinimumLength = 1)]
    [BsonElement("description")]
    [BsonRequired]
    public required string Description { get; set; } = string.Empty;

    [BsonElement("totalAvailable")]
    [BsonRequired]
    public required int TotalAvaliable { get; set; }

    [BsonElement("sold")] [BsonRequired] public required int Sold { get; set; } = 0;

    [BsonElement("enabled")]
    [BsonRequired]
    public required bool Enabled { get; set; } = false;

    [BsonElement("price")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public required decimal Price { get; set; }

    [BsonElement("discountedPrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? DiscountedPrice { get; set; }
}

public class CompanySummaryAdminView
{
    [BsonElement("id")] public required string CompanyId { get; set; }

    [BsonElement("name")] public required string CompanyName { get; set; }

    [BsonElement("image")] public required string CompanyImageUrl { get; set; }
}