using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Controllers.Events.Public
{
    [ApiController]
    [Route("e")]
    public class EventPublicController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventPublicController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<EventPublic>> GetEventPublicAsync([FromRoute] string slug)
        {
            _eventService.GetBySlugPublic();
        }
    }
}
