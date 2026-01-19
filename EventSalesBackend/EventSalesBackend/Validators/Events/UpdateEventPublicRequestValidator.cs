using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Events;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Events;

public class UpdateEventPublicRequestValidator : AbstractValidator<UpdateEventPublicRequest>
{
    public UpdateEventPublicRequestValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug cannot be empty")
            .Length(1, 30).WithMessage("Slug must be between 1 and 30 characters")
            .Matches(RegexPatterns.SlugPattern).WithMessage(RegexValidationMessages.SlugPatternMessage);
    }
}