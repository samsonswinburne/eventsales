using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace EventSalesBackend.Models
{
    public class EventHost
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("firstName")]
        [BsonRequired]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        [BsonRequired]
        public string LastName { get; set; }

        [BsonElement("birthDate")]
        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateOnly BirthDate { get; set; }


    }
}
