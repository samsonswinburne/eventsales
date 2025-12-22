namespace EventSalesBackend.Models.DTOs.Request.Events;

public readonly record struct AdminSummaryDTO
{
    public List<string> Admins { get; init; }
    public CompanySummary Summary { get; init; }
}