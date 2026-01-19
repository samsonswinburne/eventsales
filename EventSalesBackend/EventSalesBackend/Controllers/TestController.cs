using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventSalesBackend.Services.Interfaces;

namespace EventSalesBackend.Controllers;

[ApiController]
[Route("[controller]/{validator}")]
public class TestController : ControllerBase
{
    private readonly IUserClaimsService _userClaimService;

    public TestController(IUserClaimsService userClaimService)
    {
        _userClaimService = userClaimService;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        //var email = _userClaimService.GetEmail();
        //if (email is null)
        //{
        //    return BadRequest();
        //}
        //return Ok(email);
        var claims = User.Claims.Select(c => new
        {
            Type = c.Type,
            Value = c.Value
        });

        return Ok(claims);
    }
}