using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Companies;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Companies;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IUserClaimsService _userClaimsService;

    public CompanyController(IUserClaimsService userClaimsService, ICompanyService companyService)
    {
        _userClaimsService = userClaimsService;
        _companyService = companyService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<string>> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var userId = _userClaimsService.GetUserId();
        if (userId is null) return Unauthorized();

        var company = new Company
        {
            OwnerId = userId,
            Admins = [userId],
            LogoUrl = request.LogoId ?? string.Empty,
            Description = request.Description,
            Name = request.Name,
            EventIds = [],
            PostCode = request.PostCode
        };
        var result = await _companyService.CreateAsync(company);
        if (result is null) return BadRequest();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyPublic>> GetPublicAsync(string id)
    {
        var conversionSuccess = ObjectId.TryParse(id, out var companyId);
        if (!conversionSuccess) return BadRequest();
        var result = await _companyService.GetPublicAsync(companyId);
        if (result is null) return NotFound();
        return Ok(result);
    }
}