using EventSalesBackend.Models.DTOs.Request.Events;
using FluentValidation;
using MongoDB.Bson;

namespace EventSalesBackend.Validators.Events;

public class UpdateEventPublicRequestValidator : AbstractValidator<UpdateEventPublicRequest>
{
    public UpdateEventPublicRequestValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("EventId cannot be empty")
            .Length(24).WithMessage("EventId must be 24 characters")
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("EventId must be a valid eventId");
    }
}