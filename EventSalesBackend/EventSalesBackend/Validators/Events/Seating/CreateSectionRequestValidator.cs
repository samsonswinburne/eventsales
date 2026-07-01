using EventSalesBackend.Models.DTOs.Request.Events.Seating;
using FluentValidation;

namespace EventSalesBackend.Validators.Events.Seating;

public class CreateSectionRequestValidator : AbstractValidator<CreateSectionRequest>
{
    public CreateSectionRequestValidator()
    {


    }
}