using EventSalesBackend.Extensions;
using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events;
using EventSalesBackend.Models.DTOs.Response.AdminView.Event;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Events;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IEventService _eventService;
    private readonly IUserClaimsService _userClaimsService;

    public EventController(IEventService eventService, ICompanyService companyService,
        IUserClaimsService userClaimsService)
    {
        _eventService = eventService;
        _companyService = companyService;
        _userClaimsService = userClaimsService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventPublic>> GetEventByIdPublic(string id)
    {
        if (!ObjectId.TryParse(id, out var convertedId)) return BadRequest("Invalid ID format");
        try
        {
            var result = await _eventService.GetByIdPublicAsync(convertedId, "GOINGTOBEID");
            return result;
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("nearby")]
    public async Task<ActionResult<List<EventPublic>>> GetNearbyEvents([FromQuery] GetNearbyEventsRequest request)
    {
        var result = await _eventService.FindInRadiusPublicAsync(request.Latitude, request.Longitude, request.Radius);
        return result;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EventAdminView>> CreateEvent(CreateEventRequest request,
        [FromServices] IValidator<CreateEventRequest> validator)
    {
        var userId = _userClaimsService.GetUserId();

        if (userId is null) return Unauthorized();

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToErrorResponse());

        if (!ObjectId.TryParse(request.CompanyId, out var companyId)) return BadRequest();


        var adminSummaryDto = await _companyService.GetAdminSummaryAsync(companyId, userId);
        if (adminSummaryDto is null) return NotFound();

        var eventToCreate = request.ToEvent(adminSummaryDto.Value.Admins, adminSummaryDto.Value.Summary);
        await _eventService.CreateAsync(eventToCreate);
        return eventToCreate.ToAdminView();
    }
    [Authorize]
    [HttpPost("update/public")]
    public async Task<ActionResult<UpdateEventPublicResponse>> UpdateEventPublic([FromBody] UpdateEventPublicRequest request, 
        [FromServices] IValidator<UpdateEventPublicRequest> validator)
    {
        var userId = _userClaimsService.GetUserId();
        if (userId is null) return Unauthorized();

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.ToErrorResponse());

        throw new NotImplementedException();
        return Ok(new UpdateEventPublicResponse
        {

        });
    }
}