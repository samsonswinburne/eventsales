using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using PaypalServerSdk.Standard.Models;

namespace EventSalesBackend.Models;

public enum TicketStatus
{
    NotFound,
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
    [BsonElement("ticketTypeId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public required ObjectId TicketTypeId { get; set; }
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

    [BsonElement("key")]
    [BsonRequired]
    public required string Key { get; set; }
    
    [BsonElement("discount")]
    [BsonIgnoreIfNull]
    public Discount? Discount { get; set; }
    
    [BsonElement("purchasePrice")]
    [BsonRequired]
    public required decimal PurchasePrice { get; set; }
    
    [BsonElement("originalPrice")]
    [BsonRequired]
    public required decimal OriginalPrice { get; set; }
    
}


public static class TicketExtensions
{
    public static PurchaseUnitRequest ToPurchaseUnitRequest(this Ticket ticket)
    {
        decimal discountValue = ticket.Discount?.DiscountAmount ?? 0m;
        return new PurchaseUnitRequest
        {
            ReferenceId = ticket.Id.ToString(),
            Amount = new AmountWithBreakdown
            {
                CurrencyCode = "AUD",
                MValue = ticket.PurchasePrice.ToString(CultureInfo.InvariantCulture),
                Breakdown = new AmountBreakdown
                {
                    Discount = new Money
                    {
                        MValue = discountValue.ToString(CultureInfo.InvariantCulture),
                        CurrencyCode = "AUD"
                    }
                }
            },
            Shipping = new ShippingDetails
            {
                EmailAddress = ticket.CustomerEmail,
                Name = new ShippingName{FullName = ticket.CustomerName},
                PhoneNumber = new PhoneNumberWithCountryCode{CountryCode = "61", NationalNumber = ticket.CustomerPhone},
            }
        };
    }
    public static TicketPublic ToPublic(this Ticket ticket)
    {
        return new TicketPublic
        {
            Id = ticket.Id.ToString(),
            EventId = ticket.EventId.ToString(),
            TicketTypeId = ticket.TicketTypeId.ToString(),
            CustomerId = ticket.CustomerId?.ToString(),
            CustomerEmail = ticket.CustomerEmail,
            CustomerName = ticket.CustomerName,
            CustomerPhone = ticket.CustomerPhone,
            PurchaseTime = ticket.PurchaseTime,
            OrderDelivered = ticket.OrderDelivered,
            OrderDeliveryDate = ticket.OrderDeliveryDate,
            Key = ticket.Key,
            Status = ticket.Status,

            // NEW FIELDS
            PurchasePrice = ticket.PurchasePrice,
            OriginalPrice = ticket.OriginalPrice,
            Discount = ticket.Discount?.ToJsonFormat()
        };
    }

    public static TicketAdminView ToAdminView(this Ticket ticket)
    {
        return new TicketAdminView
        {
            Id = ticket.Id.ToString(),
            EventId = ticket.EventId.ToString(),
            TicketTypeId = ticket.TicketTypeId.ToString(),
            CustomerId = ticket.CustomerId?.ToString(),
            CustomerEmail = ticket.CustomerEmail,
            CustomerName = ticket.CustomerName,
            CustomerPhone = ticket.CustomerPhone,
            PurchaseTime = ticket.PurchaseTime,
            OrderDelivered = ticket.OrderDelivered,
            OrderDeliveryDate = ticket.OrderDeliveryDate,
            Status = ticket.Status,
            ScanHistory = ticket.ScanHistory.ConvertAll(ts => ts.ToJsonFormat()),

            // NEW FIELDS
            PurchasePrice = ticket.PurchasePrice,
            OriginalPrice = ticket.OriginalPrice,
            Discount = ticket.Discount?.ToJsonFormat()
        };
    }
}

public enum TicketScanAction
{
    None,
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

public static class TicketScanExtensions
{
    public static TicketScanJson ToJsonFormat(this TicketScan scan)
    {
        return new TicketScanJson
        {
            Id = scan.Id.ToString(),
            ScannerUserId = scan.ScannerUserId,
            ScanTime =  scan.ScanTime,
            Action = scan.Action
        };
    }
}