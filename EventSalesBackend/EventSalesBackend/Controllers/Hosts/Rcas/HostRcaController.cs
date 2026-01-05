using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventSalesBackend.Controllers.Hosts.Rcas
{
    [Route("me/rca")]
    [ApiController]
    public class HostRcaController : ControllerBase
    {
        private readonly IHostService _hostService;
        private readonly IUserClaimsService _userClaimsService;
        public HostRcaController(IHostService hostService, IUserClaimsService userClaimsService)
        {
            _hostService = hostService;
            _userClaimsService = userClaimsService;
        }
        // GET: api/<Rcas>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<ActionResult> Get(string? id, [FromQuery] string? status = null)
        {
            

            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();

            if (string.IsNullOrEmpty(id))
            {
                if (!ObjectId.TryParse(id, out var parsedId))
                {
                    return BadRequest("Rca Id is in an invalid format");
                }
                var singleResult =  await _hostService.GetRcaByIdAsyncProtected(parsedId, userId);
                if (singleResult is null) return NotFound();
                return Ok(singleResult);
            }

            RcaStatus? parsedStatus = Enum.TryParse<RcaStatus>(status, out var t)
                         ? t
                         : null;
            var result = await _hostService.GetRcaByHostIdStatusAsync(userId, parsedStatus);
            if(result is null) return NotFound();
            return Ok(result);

        }


        // POST api/<Rcas>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }


    }
}
