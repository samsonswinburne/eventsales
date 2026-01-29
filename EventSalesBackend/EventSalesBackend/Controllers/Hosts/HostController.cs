using System.Text.RegularExpressions;
using EventSalesBackend.Models.DTOs.Request.Hosts;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Controllers.Hosts;

[ApiController]
[Route("[controller]")]
public class HostController : ControllerBase
{
    private readonly IHostService _hostService;
    private readonly IUserClaimsService _userClaimService;

    public HostController(IHostService hostService, IUserClaimsService userClaimService)
    {
        _hostService = hostService;
        _userClaimService = userClaimService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> SignupHost([FromBody] CreateHostRequest request)
    {
        var userId = _userClaimService.GetUserId();
        if (userId is null) return Unauthorized();
        var email = _userClaimService.GetEmail();
        if (email is null) return Unauthorized();
        Console.WriteLine(email);
        if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")) return BadRequest("SOMETHING WRONG WITH EMAIL");
        // probably shouldnt be here but for now it works because email's arent inputted anywhere else (other than invite actually but thats got separate validation and this is a WIP, in the future the userClaimsService should handle email validation when signign up)

        
        var result = await _hostService.CreateHost(request, userId, email);
        if (result) return Created();

        return BadRequest();
    }
    [Authorize] // protects from easy scraping hopefully, public host info is very small but it doesn't really need to be avaliable to someone not logged in
    [HttpGet("{id}")]
    public async Task<ActionResult<HostPublic>> GetHost(string id)
    {
        var result = await _hostService.GetPublicAsync(id);
        if (result is null) return NotFound();
        return result;
    }
}