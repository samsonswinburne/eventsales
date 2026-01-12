using EventSalesBackend.Models.DTOs.Request.Companies;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Companies
{
    public class RemoveCompanyAdminRequestValidator : AbstractValidator<RemoveCompanyAdminRequest>
    {
        public RemoveCompanyAdminRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId to remove must not be empty");
                
        }
    }
}
