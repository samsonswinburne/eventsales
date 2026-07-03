using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events.Seating;
using EventSalesBackend.Models.DTOs.Validators;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Events.Seating;

[ApiController]
[Route("api/event/{EventId}/seating")]
public class EventSeatingController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IUserClaimsService _userClaimService;
    public EventSeatingController(IEventService eventService,  IUserClaimsService userClaimService)
    {
        _eventService = eventService;
        _userClaimService = userClaimService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Section>> CreateSection(
        [FromRoute] string EventId,
        [AsParameters] CreateSectionValidatorDTO requestWrapper, // Binds everything automatically
        [FromServices] IValidator<CreateSectionValidatorDTO> sectionValidator, 
        CancellationToken cancellationToken)
    {
        // Validate the entire combined context (Path + Body)
        var validationResult = await sectionValidator.ValidateAsync(requestWrapper, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var userId = _userClaimService.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        if (!ObjectId.TryParse(requestWrapper.EventId, out var validatedEventId))
        {
            return BadRequest("Invalid Event Id");
        }
        
        
        var seats = new List<Seat>();
        var SectionId = ObjectId.GenerateNewId();
        
        if (requestWrapper.Body.IsAssignedSeats == false)
        {
            for (int i = 1; i <= requestWrapper.Body.Capacity; i++)
            {
                seats.Add(
                    new Seat
                    {
                        EventId = ObjectId.Parse(requestWrapper.EventId),
                        Row = null,
                        SeatNumber = i.ToString(),
                        SectionId = SectionId,
                        TicketTypeId = ObjectId.Parse(requestWrapper.Body.TicketTypeId)
                    }
                    );
            }
        }
        if (requestWrapper.Body.Seats is not null)
        {
            foreach (var requestSeat in requestWrapper.Body.Seats)
            {
                seats.Add(
                    new Seat
                    {
                        EventId = ObjectId.Parse(requestWrapper.EventId),
                        Row = requestSeat.Row,
                        SeatNumber = requestSeat.SeatNumber,
                        SectionId = SectionId,
                        TicketTypeId = ObjectId.Parse(requestWrapper.Body.TicketTypeId)
                    }
                );
            }
        }

        var section = new Section
        {
            Id = SectionId,
            Type = requestWrapper.Body.IsAssignedSeats ? SectionType.Reserved : SectionType.General,
            Name = requestWrapper.Body.Name,
            Seats = seats,
            Capacity = requestWrapper.Body.Capacity,
            TicketTypeId = ObjectId.Parse(requestWrapper.Body.TicketTypeId)

        };
        var result = await _eventService.AddSectionProtectedAsync(userId, ObjectId.Parse(requestWrapper.EventId), section, cancellationToken);
        return Ok(result.ConvertAll(s => s.ToJsonFormat()));
    }

}