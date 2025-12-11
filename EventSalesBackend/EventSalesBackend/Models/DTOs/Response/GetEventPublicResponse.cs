using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace EventSalesBackend.Models.DTOs.Response
{
    // not completed, not yet sure if anything needs to be removed from the event
    public class GetEventPublicResponse
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
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }


    }
}
