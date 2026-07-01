using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventSalesBackend.Models;
using EventSalesBackend.Services.Implementation;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers;

[ApiController]
[Route("[controller]/{validator}")]
public class TestController : ControllerBase
{
    private readonly IUserClaimsService _userClaimService;
    private readonly ISeatLockService _seatLockService;
    public TestController(IUserClaimsService userClaimService, ISeatLockService seatLockService)
    {
        _userClaimService = userClaimService;
        _seatLockService = seatLockService;
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

    [HttpPost]
    [Authorize]
    public async Task<List<Ticket>> Post()
    {
        return new List<Ticket>();
    }
}