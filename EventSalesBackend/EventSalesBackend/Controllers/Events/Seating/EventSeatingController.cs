using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events.Seating;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Events.Seating;

[ApiController]
[Route("{eventId}/seating")]
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
    public async Task<ActionResult<Section>> CreateSection([FromRoute] string eventId, [FromBody] CreateSectionRequest request,
        [FromServices] IValidator<CreateSectionRequest> sectionValidator, CancellationToken cancellationToken)
    {
        var userId = _userClaimService.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        if (!ObjectId.TryParse(eventId, out var validatedEventId))
        {
            return BadRequest("Invalid Event Id");
        }
        
        
        var seats = new List<Seat>();
        var SectionId = ObjectId.GenerateNewId();
        
        if (request.IsAssignedSeats == false)
        {
            for (int i = 1; i <= request.Capacity; i++)
            {
                seats.Add(
                    new Seat
                    {
                        EventId = ObjectId.Parse(eventId),
                        Row = null,
                        SeatNumber = i.ToString(),
                        SectionId = SectionId,
                        TicketTypeId = ObjectId.Parse(request.TicketTypeId)
                    }
                    );
            }
        }
        if (request.Seats is not null)
        {
            foreach (var requestSeat in request.Seats)
            {
                seats.Add(
                    new Seat
                    {
                        EventId = ObjectId.Parse(eventId),
                        Row = requestSeat.Row,
                        SeatNumber = requestSeat.SeatNumber,
                        SectionId = SectionId,
                        TicketTypeId = ObjectId.Parse(request.TicketTypeId)
                    }
                );
            }
        }

        var section = new Section
        {
            Id = SectionId,
            Type = request.IsAssignedSeats ? SectionType.Reserved : SectionType.General,
            Name = request.Name,
            Seats = seats,
            Capacity = request.Capacity,
            TicketTypeId = ObjectId.Parse(request.TicketTypeId)

        };
        var result = await _eventService.AddSectionProtectedAsync(userId, ObjectId.Parse(eventId), section, cancellationToken);
        return Ok(result.ConvertAll(s => s.ToJsonFormat()));
    }

}