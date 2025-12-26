using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Events.TicketTypes;

public class CreateTicketTypeRequestValidator : AbstractValidator<CreateTicketTypeRequest>
{
    public CreateTicketTypeRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 400).WithMessage("Description must be between 10 and 400 characters")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexPatterns.NamePatternValidationMessage);
        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater or equal 0")
            .LessThanOrEqualTo(10000).WithMessage("Price must be less or equal 10000");
        RuleFor(x => x.TicketName)
            .NotNull().WithMessage("Ticket Name is required")
            .Length(1, 100).WithMessage("Ticket Name must be between 1 and 100 characters")
            .Matches(RegexPatterns.NamePattern).WithMessage(RegexPatterns.NamePatternValidationMessage);
        RuleFor(x => x.Sold)
            .NotNull().WithMessage("Sold is required")
            .GreaterThanOrEqualTo(0).WithMessage("Sold must be between 0 and 10000")
            .LessThanOrEqualTo(10000).WithMessage("Sold must be between 0 and 10000");
        RuleFor(x => x)
            .NotEmpty().WithMessage("Ticket Type is required");
        RuleFor(x => x)
            .Must(x => x.Sold <= x.TotalAvailable)
            .WithMessage("There must be more tickets avaliable than tickets that have already sold!");
        
    }
}