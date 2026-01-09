using FluentValidation;

namespace EventSalesBackend.Models.DTOs.Request.Companies
{
    public class RemoveCompanyAdminRequest
    {
        public required string userId { get; init; }
    }
}
