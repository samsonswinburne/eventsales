using System.Text.RegularExpressions;
using EventSalesBackend.Data;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Controllers.Events.Tickets;

[Route("events/tickets")]
[ApiController]
public class EventTicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IUserClaimsService _userClaimService;

    public EventTicketsController(ITicketService ticketService, IUserClaimsService userClaimsService)
    {
        _ticketService = ticketService;
        _userClaimService = userClaimsService;
    }
    
    [Authorize]
    [HttpGet("{ticketKey}")]
    public async Task<ActionResult<TicketStatusResponse>> GetStatus([FromRoute] string ticketKey, CancellationToken cancellationToken)
    {
        var userId = _userClaimService.GetUserId();
        if (userId is null) 
            return Unauthorized();
        if (string.IsNullOrWhiteSpace(ticketKey)) 
            return BadRequest("ticketKey should not be empty");
        if (ticketKey.Length != Constants.EventKeyLength)
            return BadRequest($"A key should be {Constants.EventKeyLength} characters long");
        if (!Regex.IsMatch(ticketKey, RegexPatterns.AlNumPattern))
            return BadRequest(RegexValidationMessages.AlNumPatternMessage);

        try
        {
            var result = await _ticketService.GetTicketStatusFromKey(ticketKey, userId, cancellationToken);
            return Ok(new TicketStatusResponse(result));
        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
            return BadRequest(ex.Message);
        }
    }
}