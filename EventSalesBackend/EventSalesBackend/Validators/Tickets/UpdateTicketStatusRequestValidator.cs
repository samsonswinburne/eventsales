using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Tickets;
using FluentValidation;

namespace EventSalesBackend.Validators.Tickets;

public class UpdateTicketStatusRequestValidator : AbstractValidator<UpdateTicketStatusRequest>
{
    public UpdateTicketStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(x => Enum.IsDefined(typeof(TicketStatus), x))
            .WithMessage("Invalid status")
            .GreaterThanOrEqualTo(2)
            .WithMessage("Invalid status");
            
        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("Override is required");


    }
}