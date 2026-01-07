using EventSalesBackend.Extensions;
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

            if (!string.IsNullOrEmpty(id))
            {
                if (!ObjectId.TryParse(id, out var parsedId))
                {
                    return BadRequest("Rca Id is in an invalid format");
                }
                try
                {
                    var singleResult = await _hostService.GetRcaByIdAsyncProtected(parsedId, userId);
                    if (singleResult is null) return NotFound();
                    return Ok(singleResult);
                }catch (Exception ex)
                {
                    if (ex is BaseException bex)
                    {
                        return BadRequest(bex.ToErrorResponse());
                    }
                    return BadRequest("Unspecified error " + ex.Message);
                }

                
            }

            RcaStatus? parsedStatus = Enum.TryParse<RcaStatus>(status, out var t)
                         ? t
                         : null;

            try
            {
                var results = await _hostService.GetRcaByHostIdStatusAsync(userId, parsedStatus);
                if (results is null) return NotFound();
                return Ok(results);
            }catch (Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return BadRequest(bex.ToErrorResponse());
                }
                return BadRequest("Unspecified error: " + ex.Message);
            }



        }


        // POST api/<Rcas>
        [HttpPost("{rcaId}")]
        public async Task<ActionResult> Post([FromRoute] string rcaId, [FromBody] bool verdict)
        {
            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();
            if (!ObjectId.TryParse(rcaId, out var validatedRcaId)) return BadRequest("RCA Id is in an invalid format");

            try
            {
                var success = verdict ? await _hostService.AcceptRcaAsync(validatedRcaId, userId) : await _hostService.DeclineRcaAsync(validatedRcaId, userId);

                // kind of verbose but essentially just if success then we will return Ok otherwise we will return that they failed to decline / accept the RCA
                return success ? Ok() : verdict ? BadRequest("Failed to accept RCA") : BadRequest("Failed to decline RCA");
            }
            catch(Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return BadRequest(bex.ToErrorResponse());
                }
                return BadRequest("An unspecified error occured " + ex.Message);
            }

            throw new NotImplementedException();
        }


    }
}
