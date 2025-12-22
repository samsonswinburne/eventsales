using EventSalesBackend.Data;
using EventSalesBackend.Models.DTOs.Request.Events;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Event;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company Id is required")
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Company Id must be a valid Id");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required")
            .Length(3, 200).WithMessage("Event name must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
            .Matches(RegexPatterns.NamePatternValidationMessage);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
        

        // Conditional validation for in-person events


        RuleFor(x => x.IndividualPurchaseLimit)
            .NotNull().WithMessage("Individual purchase limit is required")
            .GreaterThanOrEqualTo(0).WithMessage("Purchase limit cannot be negative");
    }
}