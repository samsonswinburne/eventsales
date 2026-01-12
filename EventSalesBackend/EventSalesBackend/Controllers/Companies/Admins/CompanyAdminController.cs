using EventSalesBackend.Extensions;
using EventSalesBackend.Models.DTOs.Request.Companies;
using EventSalesBackend.Models.DTOs.Response.PublicInfo;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventSalesBackend.Controllers.Companies.Admins
{
    [Authorize]
    [ApiController]
    [Route("company/{companyId}/admins")]
    public class CompanyAdminController : ControllerBase
    {
        private readonly IUserClaimsService _userClaimsService;
        private readonly ICompanyService _companyService;

        public CompanyAdminController(IUserClaimsService userClaimsService, ICompanyService companyService)
        {
            _userClaimsService = userClaimsService;
            _companyService = companyService;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveCompanyAdmin([FromBody] RemoveCompanyAdminRequest request, [FromRoute] string companyId,
            [FromServices] IValidator<RemoveCompanyAdminRequest> validator)
        {
            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();
            if (!ObjectId.TryParse(companyId, out var validatedCompanyId)) return BadRequest("CompanyId is invalid");
            var validatorResult = await validator.ValidateAsync(request);
            if (!validatorResult.IsValid) return BadRequest(validatorResult.ToErrorResponse());

            try
            {
                var result = await _companyService.RemoveAdminProtectedAsync(validatedCompanyId, userId, request.UserId);
                return result ? Ok() : BadRequest();
            }catch(Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return Ok(bex.ToErrorResponse());
                }
                return BadRequest("Unspecified error! " + ex.Message);
            }

        }

        [HttpPost("owner")]
        public async Task<ActionResult> SetOwner([FromBody] SetCompanyOwnerRequest request, [FromRoute] string companyId,
            [FromServices] IValidator<SetCompanyOwnerRequest> validator)
        {
            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();
            if (!ObjectId.TryParse(companyId, out var validatedCompanyId)) return BadRequest("CompanyId is invalid");
            var validatorResult = await validator.ValidateAsync(request);
            if (!validatorResult.IsValid) return BadRequest(validatorResult.ToErrorResponse());

            try
            {
                var result = await _companyService.SetOwnerProtectedAsync(validatedCompanyId, userId, request.UserId);
                return result ? Ok() : BadRequest();
            }catch(Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return BadRequest(bex.ToErrorResponse());
                }
                return BadRequest("Unspecified error: " + ex.Message);
            }
        }

        [HttpPost(Name = "Invite admin to company")]
        public async Task<ActionResult<RequestCompanyAdminPublic>> RequestCompanyAdminAsync([FromBody] RequestCompanyAdminRequest request,
        [FromRoute] string companyId, [FromServices] IValidator<RequestCompanyAdminRequest> validator)
        {

            var userId = _userClaimsService.GetUserId();
            if (userId is null) return Unauthorized();
            if (!ObjectId.TryParse(companyId, out var validatedCompanyId)) return BadRequest("CompanyId is invalid");
            var validatorResult = await validator.ValidateAsync(request);
            if (!validatorResult.IsValid) return BadRequest(validatorResult.ToErrorResponse());

            try
            {
                var result =
                    await _companyService.InviteAdminAsync(userId, validatedCompanyId, request.AdminRequestReceiverEmail);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is BaseException bex)
                {
                    return BadRequest(bex.ToErrorResponse());
                }
                return BadRequest("Unspecified error: " + ex.Message);
            }
        }
    }
}
