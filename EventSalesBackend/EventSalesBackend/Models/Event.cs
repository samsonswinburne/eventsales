using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EventSalesBackend.Models.DTOs.Response.AdminView.Event;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Models;

[BsonIgnoreExtraElements]
public class Event // event should be split up into in person event and digital event, in person event requires different things than digital event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("companySummary")] public required CompanySummary HostCompanySummary { get; set; }

    [BsonElement("name")] [BsonRequired] public required string Name { get; set; }
    [BsonElement("slug")][BsonIgnoreIfNull] public string? Slug { get; set; }
    [BsonElement("description")] public string? Description { get; set; }

    [BsonElement("ticketTypes")]
    [BsonRequired]
    public required List<TicketType> TicketTypes { get; set; } = new();

    [BsonElement("summary")]
    [BsonRequired]
    public required TicketSummary Summary { get; set; }

    [BsonElement("photo")] [BsonRequired] public required string Photo { get; set; }

    [BsonElement("postCode")]
    [BsonRequired]
    public string? PostCode { get; set; }

    [BsonElement("inPersonEvent")]
    [BsonRequired]
    public required bool InPersonEvent { get; set; }

    [BsonElement("venueAddress")]
    [BsonIgnoreIfNull]
    public string? VenueAddress { get; set; }

    [BsonElement("individualPurchaseLimit")]
    [BsonRequired]
    public int IndividualPurchaseLimit { get; set; } = 0;

    [BsonElement("venueLocation")]
    [BsonIgnoreIfNull]
    public GeoJsonPoint<GeoJson2DGeographicCoordinates>? VenueLocation { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public EventStatus Status { get; set; } = EventStatus.Draft;

    [BsonElement("startDate")]
    [BsonRequired]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    [BsonRequired]
    public DateTime EndDate { get; set; }

    [BsonElement("created")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [BsonElement("admins")] [BsonRequired] public required List<string> Admins { get; set; }
}

public static class EventExtensions
{
    public static EventPublic ToPublic(this Event eventToConvert)
    {
        return new EventPublic
        {
            Id = eventToConvert.Id.ToString(),
            HostCompanySummary = eventToConvert.HostCompanySummary.ToJson(),
            Name = eventToConvert.Name,
            Description = eventToConvert.Description,
            TicketTypes = eventToConvert.TicketTypes.ConvertAll(e => e.ToPublic()),
            Summary = eventToConvert.Summary,
            Photo = eventToConvert.Photo,
            PostCode = eventToConvert.PostCode, // to make public the event should have a postcode?
            InPersonEvent = eventToConvert.InPersonEvent,
            VenueAddress = eventToConvert.VenueAddress,
            IndividualPurchaseLimit = eventToConvert.IndividualPurchaseLimit,
            VenueLocation = eventToConvert.VenueLocation?.Coordinates != null
                ? new JsonVenueLocation
                {
                    Latitude = eventToConvert.VenueLocation.Coordinates.Latitude,
                    Longitude = eventToConvert.VenueLocation.Coordinates.Longitude
                }
                : null,
            Status = eventToConvert.Status,
            StartDate = eventToConvert.StartDate,
            EndDate = eventToConvert.EndDate
        };
    }

    public static EventAdminView ToAdminView(this Event eventToConvert)
    {
        return new EventAdminView
        {
            Id = eventToConvert.Id.ToString(),
            HostCompanySummary = eventToConvert.HostCompanySummary.ToJson(),
            Name = eventToConvert.Name,
            Description = eventToConvert.Description,
            TicketTypes = eventToConvert.TicketTypes.Select(tt => new TicketTypeAdminView
            {
                Id = tt.Id.ToString(),
                Name = tt.Name,
                Description = tt.Description,
                TotalAvaliable = tt.TotalAvaliable,
                Sold = tt.Sold,
                Enabled = tt.Enabled,
                Price = tt.Price,
                DiscountedPrice = tt.DiscountedPrice
            }).ToList(),
            Summary = eventToConvert.Summary,
            Photo = eventToConvert.Photo,
            PostCode = eventToConvert.PostCode,
            InPersonEvent = eventToConvert.InPersonEvent,
            VenueAddress = eventToConvert.VenueAddress,
            IndividualPurchaseLimit = eventToConvert.IndividualPurchaseLimit,
            VenueLocation = eventToConvert.VenueLocation?.Coordinates != null
                ? new JsonVenueLocation
                {
                    Latitude = eventToConvert.VenueLocation.Coordinates.Latitude,
                    Longitude = eventToConvert.VenueLocation.Coordinates.Longitude
                }
                : null,
            Status = eventToConvert.Status,
            StartDate = eventToConvert.StartDate,
            EndDate = eventToConvert.EndDate,
            Created = eventToConvert.Created,
            Admins = eventToConvert.Admins
        };
    }
}

public enum EventStatus
{
    Draft,
    Published,
    Cancelled,
    Completed
}

public class TicketType
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

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
    public DateTime? DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }
}

public static class TicketTypeExtensions
{
    public static TicketTypePublic ToPublic(this TicketType ticketTypeToConvert)
    {
        return new TicketTypePublic
        {
            Id = ticketTypeToConvert.Id.ToString(),
            Name = ticketTypeToConvert.Name,
            Description = ticketTypeToConvert.Description,
            TotalAvaliable = ticketTypeToConvert.TotalAvaliable,
            Sold = ticketTypeToConvert.Sold,
            Price = ticketTypeToConvert.Price,
            DiscountedPrice = ticketTypeToConvert.DiscountedPrice
        };
    }
}

public class TicketSummary
{
    [SetsRequiredMembers]
    public TicketSummary()
    {
        TotalCapacity = 0;
        TotalSold = 0;
        TotalSales = 0;
    }

    [BsonElement("totalCapacity")]
    [BsonRequired]
    public required int TotalCapacity { get; set; }

    [BsonElement("totalSold")]
    [BsonRequired]
    public required int TotalSold { get; set; }

    [BsonElement("totalSales")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public required int TotalSales { get; set; }
}

public class CompanySummary
{
    [BsonElement("companyId")] public required ObjectId CompanyId { get; set; }

    [BsonElement("name")] public required string CompanyName { get; set; }

    [BsonElement("image")] public required string CompanyImageUrl { get; set; }
}
public static class CompanySummaryExtensions
{
    public static CompanySummaryJson ToJson(this CompanySummary cs)
    {
        return new CompanySummaryJson
        {
            CompanyId = cs.CompanyId.ToString(),
            CompanyImageUrl = cs.CompanyImageUrl,
            CompanyName = cs.CompanyName
        };
    }
}