using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Models
{
    public class Event
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required List<TicketType> TicketTypes { get; set; } = new List<TicketType>();
        public required TicketSummary Summary { get; set; }
        public required string Photo {  get; set; }

        public required string PostCode { get; set; }
        public required bool InPersonEvent { get; set; }
        public string? VenueAddress { get; set; }
        public required int IndividualPurchaseLimit { get; set; } = 0;

        public GeoJsonPoint<GeoJson2DGeographicCoordinates>? VenueLocation { get; set; }
    }
    public class TicketType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required int TotalAvaliable { get; set; }
        public required int Sold { get; set; } = 0;
        public required bool Enabled { get; set; } = false;
        public required decimal Price { get; set; }

    }
    public class TicketSummary
    {
        public required int TotalCapacity { get; set; } = 0;
        public required int TotalSold { get; set; } = 0;
        public required int TotalSales { get; set; } = 0;
    }
}
