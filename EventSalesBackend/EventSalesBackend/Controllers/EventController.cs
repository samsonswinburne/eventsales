using EventSalesBackend.Models.DTOs.Request;
using EventSalesBackend.Models.DTOs.Response;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
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
                return Unauthorized();
            }
        }
        [HttpGet("nearby")]
        public async Task<ActionResult<List<EventPublic>>> GetNearbyEvents([FromQuery] GetNearbyEventsRequest request)
        {
            var result = await _eventService.FindInRadiusPublicAsync(request.Latitude, request.Longitude, request.Radius);
            return result;
        }
        
    }
}
