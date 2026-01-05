using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string? status = null)
        {
            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();
            RcaStatus? parsedStatus = Enum.TryParse<RcaStatus>(status, out var t)
                         ? t
                         : null;
            var result = await _hostService.GetByHostIdStatusAsync(userId, parsedStatus);
            if(result is null) return NotFound();
            return Ok(result);

        }

        // GET api/<Rcas>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<Rcas>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }


    }
}
