using MongoDB.Bson.Serialization.Attributes;
namespace EventSalesBackend.Models
{
    public class Host
    {
        [BsonId]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }


    }
}
