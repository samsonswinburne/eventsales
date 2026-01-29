using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventSalesBackend.Services.Interfaces;
using MongoDB.Bson;

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

    [HttpGet("writeTicket")]
    public async Task<IActionResult> WriteTicket(ITicketService ticketService, ICryptoService cryptoService, CancellationToken cancellationToken)
    {
        var x = await ticketService.CreateTicket(new ObjectId("697b442f905b02f231d2f2a3"), new ObjectId("697b448f905b02f231d2f2a4"), null, "testemail@gmail.com", "johnny smith", null,
            cryptoService, cancellationToken);
        return Ok(x);
    }
}