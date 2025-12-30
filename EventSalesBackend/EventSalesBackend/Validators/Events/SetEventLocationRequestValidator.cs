using EventSalesBackend.Models.DTOs.Request.Events;
using FluentValidation;

namespace EventSalesBackend.Validators.Events
{
    public class SetEventLocationRequestValidator : AbstractValidator<UpdateEventLocationRequest>
    {
        public SetEventLocationRequestValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Latitude must be set")
                .GreaterThan(-43.64).WithMessage("Latitude should be inside Australia")
                .LessThan(-9.14).WithMessage("Latitude should be inside Australia");
            RuleFor(x => x.Longitude)
                .NotEmpty().WithMessage("Longitude must be set")
                .GreaterThan(113.15).WithMessage("Longitude should be inside Australia")
                .LessThan(153.6).WithMessage("Longitude should be inside Australia");
        }
    }
}
