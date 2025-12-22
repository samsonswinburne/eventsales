namespace EventSalesBackend.Models.DTOs.Data;

public readonly record struct AdminSummaryDTO
{
    public List<string> Admins { get; init; }
    public CompanySummary Summary { get; init; }
}