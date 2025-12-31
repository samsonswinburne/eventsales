using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EventSalesBackend.Models;

public class EventHost
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required string Id { get; set; }

    [BsonElement("firstName")]
    [BsonRequired]
    public required string FirstName { get; set; }

    [BsonElement("lastName")]
    [BsonRequired]
    public required string LastName { get; set; }

    [BsonElement("birthDate")]
    [BsonRequired]
    public DateOnly BirthDate { get; set; }

    public bool OnBoardingCompleted { get; set; } = false;

    [JsonIgnore]
    [BsonElement("email")]
    [BsonRequired]
    public required string Email { get; init; }
}