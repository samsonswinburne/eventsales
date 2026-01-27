using EventSalesBackend.Models.DTOs.Request.Tickets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace EventSalesBackend.Models;

public enum TicketStatus
{
    Pending,
    Active,
    GrantedEntry,
    DeniedEntry
}
public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("eventId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public required ObjectId EventId { get; set; }

    [BsonElement("customerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public ObjectId? CustomerId { get; set; }

    [BsonElement("customerEmail")]
    [BsonRequired]
    public required string CustomerEmail { get; set; }

    [BsonElement("customerName")]
    [BsonRequired]
    public required string CustomerName { get; set; }

    [BsonElement("customerPhone")]
    [BsonIgnoreIfNull]
    public string? CustomerPhone { get; set; } = null;

    [BsonElement("purchaseTime")]
    [BsonRequired]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public required DateTime PurchaseTime { get; set; }

    [BsonElement("orderDelivered")]
    [BsonRequired]
    public required bool OrderDelivered { get; set; } = false;

    [BsonElement("orderDeliveryDate")]
    [BsonIgnoreIfNull]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? OrderDeliveryDate { get; set; } = null;

    [BsonElement("status")]
    [BsonRequired]
    public TicketStatus Status { get; set; } = TicketStatus.Pending;
    [BsonElement("scanHistory")]
    [BsonRequired]
    public List<TicketScan> ScanHistory { get; set; } = [];

}


public static class TicketExtensions
{
    public static TicketPublic ToPublic(this Ticket ticket)
    {
        return new TicketPublic
        {
            Id = ticket.Id.ToString(),
            EventId = ticket.EventId.ToString(),
            CustomerId = ticket.CustomerId?.ToString(),
            CustomerEmail = ticket.CustomerEmail,
            CustomerName = ticket.CustomerName,
            CustomerPhone = ticket.CustomerPhone,
            PurchaseTime = ticket.PurchaseTime,
            OrderDelivered = ticket.OrderDelivered,
            OrderDeliveryDate = ticket.OrderDeliveryDate
        };
    }
}

public enum TicketScanAction
{
    ValidateStatus,
    ValidateApproveEntry,
    ValidateDenyEntry
}

public class TicketScan
{
    [SetsRequiredMembers]
    public TicketScan(string scannerUserId, TicketScanAction action)
    {
        Id = ObjectId.GenerateNewId();
        ScanTime = DateTime.UtcNow;
        ScannerUserId = scannerUserId;
        Action = action;
    }

    public required ObjectId Id { get; set; }
    public required DateTime ScanTime { get; set; }
    public string? ScannerUserId { get; set; } // not entirely sure what this will look like right now
    public required TicketScanAction Action { get; set; }

}