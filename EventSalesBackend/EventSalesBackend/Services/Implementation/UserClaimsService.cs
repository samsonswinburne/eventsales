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
    public string? GetEmail()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        var email =  user?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        if(email is not null) return email;
        var userId = GetUserId();
        if (userId is null) return null;
        var type = userId.Split("|")[0];
        switch (type)
        {
            case "auth0":
                return user?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            case "google-oauth2":
                return user?.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value + "@gmail.com";
            default:
                return null;
        }
    }
    public string? GetRoles()
    {
        throw new NotImplementedException();
    }
}