using EventSalesBackend.Extensions;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Events.TicketTypes;

[ApiController]
[Route("event/{eventId}/ticket-types")]
public class TicketTypesController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IUserClaimsService _userClaimsService;

    public TicketTypesController(IEventService eventService, IUserClaimsService userClaimsService)
    {
        _eventService = eventService;
        _userClaimsService = userClaimsService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TicketType>> AddTicketType([FromRoute] string eventId, [FromBody] CreateTicketTypeRequest request,
        [FromServices] IValidator<CreateTicketTypeRequest> validator)
    {
        var userId = _userClaimsService.GetUserId();
        if (userId is null) return Unauthorized();

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid) return BadRequest(validationResult.ToErrorResponse());

        if (!ObjectId.TryParse(eventId, out var parsedEventId)) return BadRequest(eventId); // this isn't very good, should find a way to make it same format as validationResult error
        
        
        var result = await _eventService.AddTicketTypeAsync(parsedEventId, userId, request.ToTicketType());
        if (!result)
            // could occur because 404, unauthorised, more. Difficult to reason why because its just 1 query
            return BadRequest();
        return Ok(); // perhaps should be changed to return the ticketType or the ticketId
        // so that if its correct the user can then operate on it correctly
    }

    [HttpGet("{ticketId}")]
    public async Task<ActionResult<TicketType>> GetTicketType([FromQuery] string ticketId)
    {
        throw new NotImplementedException();        
    }
}