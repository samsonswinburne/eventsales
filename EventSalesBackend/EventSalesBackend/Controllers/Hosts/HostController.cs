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

        var result = await _hostService.CreateHost(request, userId);
        if (result) return Created();

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HostPublic>> GetHost(string id)
    {
        var result = await _hostService.GetPublicAsync(id);
        if (result is null) return NotFound();
        return result;
    }
}