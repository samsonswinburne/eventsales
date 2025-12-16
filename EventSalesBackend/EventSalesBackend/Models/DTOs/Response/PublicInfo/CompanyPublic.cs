

namespace EventSalesBackend.Models.DTOs.Response.PublicInfo;

public class CompanyPublic
{
    public required string Id { get; set; }


    public required string LogoUrl { get; set; } = string.Empty;


    public required List<string> EventIds { get; set; } = new List<string>();


    public string? Description { get; set; }
    
    public required string PostCode { get; set; }


    public required string Name { get; set; }
}