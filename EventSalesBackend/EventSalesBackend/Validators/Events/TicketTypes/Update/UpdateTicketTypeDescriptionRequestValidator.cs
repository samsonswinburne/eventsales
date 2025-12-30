using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes.Update;
using FluentValidation;

namespace EventSalesBackend.Validators.Events.TicketTypes.Update;

public class UpdateTicketTypeDescriptionRequestValidator :  AbstractValidator<UpdateTicketTypeDescriptionRequest>
{
    public UpdateTicketTypeDescriptionRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 400).WithMessage("Description must be between 10 and 400 characters")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexValidationMessages.NamePatternMessage);
    }
}