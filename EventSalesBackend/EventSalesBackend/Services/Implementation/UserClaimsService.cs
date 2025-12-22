using System.Security.Claims;
using EventSalesBackend.Services.Interfaces;

namespace EventSalesBackend.Services.Implementation;

public class UserClaimsService : IUserClaimsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetRoles()
    {
        throw new NotImplementedException();
    }
}