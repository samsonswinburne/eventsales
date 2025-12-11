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
        public async Task<ActionResult<GetEventPublicResponse>> GetEventByIdPublic(string id)
        {
            if (!ObjectId.TryParse(id, out var convertedId))
            {
                return BadRequest("Invalid ID format");   
            }
            try
            {
                var result = await _eventService.GetByIdPublicAsync(convertedId, new ObjectId());
                return result;
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
            

        }
    }
}
