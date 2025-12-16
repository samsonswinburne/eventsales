using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Models
{
    [BsonIgnoreExtraElements]
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("companySummary")]
        public required CompanySummary HostCompanySummary { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public required string Name { get; set; }

        [BsonElement("description")]
        [BsonRequired]
        public required string Description { get; set; }

        [BsonElement("ticketTypes")]
        [BsonRequired]
        public required List<TicketType> TicketTypes { get; set; } = new List<TicketType>();

        [BsonElement("summary")]
        [BsonRequired]
        public required TicketSummary Summary { get; set; }

        [BsonElement("photo")]
        [BsonRequired]
        public required string Photo { get; set; }

        [BsonElement("postCode")]
        [BsonRequired]
        public required string PostCode { get; set; }

        [BsonElement("inPersonEvent")]
        [BsonRequired]
        public required bool InPersonEvent { get; set; }

        [BsonElement("venueAddress")]
        [BsonIgnoreIfNull]
        public string? VenueAddress { get; set; }

        [BsonElement("individualPurchaseLimit")]
        [BsonRequired]
        public required int IndividualPurchaseLimit { get; set; } = 0;

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

        [BsonElement("admins")]
        [BsonRequired]
        public required List<string> Admins { get; set; }

    }

    public static class EventExtensions
    {
        public static EventPublic ToPublic(this Event eventToConvert)
        {
            return new EventPublic
            {
                Id = eventToConvert.Id.ToString(),
                HostCompanySummary = eventToConvert.HostCompanySummary,
                Name = eventToConvert.Name,
                Description = eventToConvert.Description,
                TicketTypes = eventToConvert.TicketTypes,
                Summary = eventToConvert.Summary,
                Photo = eventToConvert.Photo,
                PostCode = eventToConvert.PostCode,
                InPersonEvent = eventToConvert.InPersonEvent,
                VenueAddress = eventToConvert.VenueAddress,
                IndividualPurchaseLimit = eventToConvert.IndividualPurchaseLimit,
                // dereference of a possible null reference, need to ensure that the venue coordinates aren't null
                // which they should never be because the object can be null but coordinates should be required
                VenueLocation = new JsonVenueLocation{Latitude = eventToConvert.VenueLocation.Coordinates.Latitude, Longitude = eventToConvert.VenueLocation.Coordinates.Longitude},
                Status = eventToConvert.Status,
                StartDate = eventToConvert.StartDate,
                EndDate = eventToConvert.EndDate
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
        public required string Name { get; set; }

        [BsonElement("description")]
        [BsonRequired]
        public required string Description { get; set; } = string.Empty;

        [BsonElement("totalAvailable")]
        [BsonRequired]
        public required int TotalAvaliable { get; set; }

        [BsonElement("sold")]
        [BsonRequired]
        public required int Sold { get; set; } = 0;

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

    public class TicketSummary
    {
        [BsonElement("totalCapacity")]
        [BsonRequired]
        public required int TotalCapacity { get; set; } = 0;

        [BsonElement("totalSold")]
        [BsonRequired]
        public required int TotalSold { get; set; } = 0;

        [BsonElement("totalSales")]
        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public required int TotalSales { get; set; } = 0;
    }
    public class CompanySummary
    {
        [BsonElement("id")]
        public required ObjectId CompanyId { get; set; }
        [BsonElement("name")]
        public required string CompanyName { get; set; }
        [BsonElement("image")]
        public required string CompanyImageUrl { get; set; }
    }
}