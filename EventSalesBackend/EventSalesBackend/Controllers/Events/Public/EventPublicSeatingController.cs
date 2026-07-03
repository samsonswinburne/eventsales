using System.Text.Json;
using EventSalesBackend.Realtime.Seating;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Events.Public;

[ApiController]
[Route("api/e/{eventId}/seating")]
public class EventPublicSeatingController : ControllerBase
{
    [HttpGet("stream")]
    public async Task Stream(
        string eventId,
        [FromServices] ISeatUpdateHub hub,
        CancellationToken cancellationToken
    )
    {
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers["X-Accel-Buffering"] = "no"; // important for nginx
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        // still doesn't have actual checking of whether the event exists and is currently selling tickets
        if (!ObjectId.TryParse(eventId, out var eventIdValidated))
        {
            // need a better return
            Response.StatusCode = 400;
            await Response.Body.FlushAsync(cancellationToken);
            return;
            
        }

        await foreach (var update in hub.SubscribeAsync(eventIdValidated))
        {
            
            var json = JsonSerializer.Serialize(update.ToJsonFormat());

            await Response.WriteAsync($"data: {json}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
        
    }
    
}