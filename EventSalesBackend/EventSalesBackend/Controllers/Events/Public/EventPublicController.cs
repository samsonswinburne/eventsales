using EventSalesBackend.Extensions;
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
            if (String.IsNullOrEmpty(slug) || slug.Length > 30)
            {
                return BadRequest(slug);
            }
            try
            {
                var result =  await _eventService.GetBySlugPublicProtected(slug);
                return result is not null ? Ok(result) : NotFound(slug);
            }
            catch (Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return BadRequest(bex.ToErrorResponse());
                }
                return BadRequest("Unspecified error: " +  ex.Message);
            }
        }
    }
}
