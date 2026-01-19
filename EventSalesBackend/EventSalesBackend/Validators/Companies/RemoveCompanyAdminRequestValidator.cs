using EventSalesBackend.Models.DTOs.Request.Companies;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Companies
{
    public class SetCompanyOwnerRequestValidator
        : AbstractValidator<SetCompanyOwnerRequest>
    {
        public SetCompanyOwnerRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId to make owner must not be empty");
                
        }
    }
}
