using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public required ObjectId EventId {get; set; }

        public ObjectId? CustomerId {  get; set; }

        public required string CustomerEmail { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerPhone { get; set; } = null;

        public required DateTime PurchaseTime { get; set; }

        public required bool OrderDelivered { get; set; } = false;
        public DateTime OrderDeliveryDate { get; set; } = DateTime.MinValue;
        
    }
}
