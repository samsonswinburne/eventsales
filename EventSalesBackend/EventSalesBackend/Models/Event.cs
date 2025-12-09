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
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }

        [BsonElement("created")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Created { get; set; } = DateTime.UtcNow;
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
}