using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes.Update;
using FluentValidation;

namespace EventSalesBackend.Validators.Events.TicketTypes.Update;

public class UpdateTicketTypeNameRequestValidator : AbstractValidator<UpdateTicketTypeNameRequest>
{
    public UpdateTicketTypeNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexPatterns.NamePatternValidationMessage)
            .Length(1, 100).WithMessage("Ticket Name must be between 1 and 100 characters");
    }   
}