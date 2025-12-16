using EventSalesBackend.Models.DTOs.Request;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class HostController : ControllerBase
{
    private readonly IHostService _hostService;
    public HostController(IHostService hostService)
    {
        _hostService = hostService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> SignupHost([FromBody] CreateHostRequest request)
    {
        return Ok();
    } 
    
    
}