using System.Collections.Specialized;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models;

public class Venue
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("publicVenueProfile")]
    public required bool PublicVenueProfile { get; set; }
    [BsonElement("slug")]
    public string? Slug { get; set; }
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("addressLine1")]
    public required string AddressLine1 { get; set; }
    [BsonElement("postcode")]
    public required string Postcode { get; set; }
    [BsonElement("suburb")]
    public required string Suburb { get; set; }
    [BsonElement("state")]
    public required string State { get; set; }
    
}

public class TemplateSeat
{
    public required string Row { get; set; }
    public required string Number { get; set; }
}
public enum SectionType
{
    Reserved,
    General
}
public class TemplateSection
{
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("type")]
    public required SectionType Type { get; set; }
    [BsonElement("seats")]
    public List<TemplateSeat>? Seats { get; set; }
    [BsonElement("capacity")]
    public int? Capacity { get; set; }
}

public class LayoutTemplate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("sections")]
    public required List<TemplateSection> Sections { get; set; }
}