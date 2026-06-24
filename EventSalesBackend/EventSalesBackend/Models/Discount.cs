using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models;

public enum DiscountType
{
    Percentage,
    Amount
}

public class Discount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonElement("ticketTypeId")]
    public required ObjectId  TicketTypeId { get; set; }
    [BsonElement("eventId")]
    public ObjectId? EventId { get; set; }
    [BsonElement("name")] public required string Name { get; set; }
    [BsonElement("description")] public string? Description { get; set; }
    [BsonElement("discountType")] public DiscountType DiscountType { get; set; }
    [BsonElement("discountAmount")] public required decimal DiscountAmount { get; set; }    
    [BsonElement("discountStarts")] public required DateTime DiscountStarts { get; set; }
    [BsonElement("discountEnds")] public DateTime? DiscountEnds { get; set; }
}

public class DiscountJson
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public required decimal DiscountAmount { get; set; }
    public required DateTime DiscountStarts { get; set; }
    public DateTime? DiscountEnds { get; set; }
}
public static class DiscountExtensions
{
    public static DiscountJson ToJsonFormat(this Discount discount)
    {
        return new DiscountJson
        {
            Id = discount.Id.ToString(),
            Name = discount.Name,
            Description = discount.Description,
            DiscountType = discount.DiscountType,
            DiscountAmount = discount.DiscountAmount,
            DiscountStarts = discount.DiscountStarts,
            DiscountEnds = discount.DiscountEnds,
        };
    }

    public static decimal GetDiscount(this Discount discount, decimal originalPrice)
    {
        return discount.DiscountType == DiscountType.Amount ? discount.DiscountAmount : Decimal.Multiply(discount.DiscountAmount,  originalPrice);
    }
    public static decimal GetPrice(this Discount discount, decimal originalPrice)
    {
        return discount.DiscountType == DiscountType.Amount ? decimal.Subtract(originalPrice, discount.DiscountAmount) : decimal.Subtract(originalPrice, Decimal.Multiply(discount.DiscountAmount,  originalPrice));
    }
}