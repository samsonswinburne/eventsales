using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventSalesBackend.Controllers;

[ApiController]
[Route("[controller]/{validator}")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //var emailClaim = User.Claims.Select(c => n)
        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        return Ok(emailClaim);
        
    }
}