namespace EventSalesBackend.Models.DTOs.Request.Companies;

public class CreateCompanyRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? LogoId { get; set; }
    public required string PostCode { get; set; }
}
    