using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Events;
using EventSalesBackend.Models.DTOs.Response.AdminView;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ICompanyService _companyService;
        private readonly IUserClaimsService _userClaimsService;
        public EventController(IEventService eventService,  ICompanyService companyService,  IUserClaimsService userClaimsService)
        {
            _eventService = eventService;
            _companyService = companyService;
            _userClaimsService = userClaimsService;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<EventPublic>> GetEventByIdPublic(string id)
        {
            if (!ObjectId.TryParse(id, out var convertedId))
            {
                return BadRequest("Invalid ID format");
            }
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
        // for now its a bool but return type should be changed later
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EventAdminView>> CreateEvent(CreateEventRequest request)
        {
            var userId = _userClaimsService.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            if (!ObjectId.TryParse(request.CompanyId, out var companyId))
            {
                return BadRequest();
            }
            
            
            var adminSummaryDto = await _companyService.GetAdminSummaryAsync(companyId, userId);
            if (adminSummaryDto is null)
            {
                return NotFound();
            }
            
            var eventToCreate = request.ToEvent(adminSummaryDto.Value.Admins, adminSummaryDto.Value.Summary);
            await _eventService.CreateAsync(eventToCreate);
            return eventToCreate.ToAdminView();
        }

        [Authorize]
        [HttpPost("addTicketType")]
        public async Task<ActionResult<TicketType>> AddTicketType([FromBody] CreateTicketTypeRequest request)
        { 
            var userId = _userClaimsService.GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            if (!ObjectId.TryParse(request.EventId, out var eventId))
            {
                return BadRequest("Event ID is invalid");
            }
            
            var result = await _eventService.AddTicketTypeAsync(eventId, userId, request.ToTicketType());
            if (!result)
            {
                // could occur because 404, unauthorised, more. Difficult to reason why because its just 1 query
                return BadRequest();
            }
            return Ok(); // perhaps should be changed to return the ticketType or the ticketId
            // so that if its correct the user can then operate on it correctly
        }
    }
}
