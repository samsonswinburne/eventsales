using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.Contracts;

namespace EventSalesBackend.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("ownerId")]
        public required string OwnerId { get; set; }

        [BsonElement("admins")]
        public required List<string> Admins { get; set; } = new List<string>();

        [BsonElement("logo")]
        public required string LogoUrl { get; set; } = string.Empty;

        [BsonElement("eventIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required List<string> EventIds { get; set; } = new List<string>();

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("postcode")]
        public required string PostCode { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }

        

    }
}
