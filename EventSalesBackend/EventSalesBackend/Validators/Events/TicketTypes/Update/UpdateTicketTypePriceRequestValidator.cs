using EventSalesBackend.Models.DTOs.Request.Events.TicketTypes.Update;
using FluentValidation;

namespace EventSalesBackend.Validators.Events.TicketTypes.Update;

public class UpdateTicketTypePriceRequestValidator : AbstractValidator<UpdateTicketTypePriceRequest>
{
    public UpdateTicketTypePriceRequestValidator()
    {
        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price must not be null")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");
    }
}