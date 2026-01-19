using FluentValidation;

namespace EventSalesBackend.Models.DTOs.Request.Companies
{
    public class RemoveCompanyAdminRequest
    {
        public required string UserId { get; init; }
    }
}
