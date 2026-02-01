using System.Text.RegularExpressions;
using EventSalesBackend.Data;
using EventSalesBackend.Extensions;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Tickets;
using EventSalesBackend.Models.DTOs.Response.AdminView.Tickets;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
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
    [HttpPatch("{ticketKey}")]
    public async Task<ActionResult<UpdateTicketStatusResponse>> UpdateTicketStatus([FromRoute] string ticketKey,
        [FromBody] UpdateTicketStatusRequest request, IValidator<UpdateTicketStatusRequest> validator, CancellationToken cancellationToken)
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
        
        
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToErrorResponse());
        
        var statusToSet = (TicketStatus)request.Status;

        try
        {
            var ticketStatus = await _ticketService.GetTicketStatusFromKey(ticketKey, userId, cancellationToken);
            if (ticketStatus == TicketStatus.Active || ticketStatus == TicketStatus.DeniedEntry ||
                ticketStatus == TicketStatus.GrantedEntry) //  extra valdation but shouldn't be needed hopefully
            {
                var result = await _ticketService.UpdateStatusByKeyProtected(ticketKey, statusToSet, userId,
                    request.Override, cancellationToken);
                return result == statusToSet
                    ? Ok(new UpdateTicketStatusResponse { Status = result })
                    : new UpdateTicketStatusResponse { Status = result };
            }
            else
            {
                return BadRequest("Invalid ticket status");
            }
        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
            return null;
        }
        
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
            return BadRequest(ex.Message);
        }
    }
}