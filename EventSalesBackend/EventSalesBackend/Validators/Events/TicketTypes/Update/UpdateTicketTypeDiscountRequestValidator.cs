using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes.Update;
using FluentValidation;

namespace EventSalesBackend.Validators.Events.TicketTypes.Update;

public class UpdateTicketTypeDiscountRequestValidator : AbstractValidator<UpdateTicketTypeDiscountRequest>
{
    public UpdateTicketTypeDiscountRequestValidator()
    {
        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Price must not be null"); // discount price can be higher than regular price haha
        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.StartDate.HasValue)
            .WithMessage("Start date must be greater than current date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.EndDate.HasValue)
            .WithMessage("Start date must be greater than current date");
        
        RuleFor(x => x.MaximumQuantity)
            .GreaterThan(0)
            .When(x => x.MaximumQuantity.HasValue)
            .WithMessage("Maximum quantity must be greater than 0");
    }
}