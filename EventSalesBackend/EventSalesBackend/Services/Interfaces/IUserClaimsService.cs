namespace EventSalesBackend.Services.Interfaces;

public interface IUserClaimsService
{
    string? GetUserId();
    string? GetRoles();
    string? GetEmail();
}