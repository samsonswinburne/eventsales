using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Companies;
using FluentValidation;

namespace EventSalesBackend.Validators.Companies;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(0, 25).WithMessage("Name must be between 0 and 25 characters")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexPatterns.NamePatternValidationMessage);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(0, 150).WithMessage("Description must be between 0 and 25 characters")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexPatterns.NamePatternValidationMessage);
        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("PostCode is required")
            .Length(4).WithMessage("PostCode must contain 4 digits")
            .Matches(@"\d").WithMessage("PostCode must contain 4 digits");
    }
}