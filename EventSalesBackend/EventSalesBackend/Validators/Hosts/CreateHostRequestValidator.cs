using EventSalesBackend.Models.DTOs.Request.Hosts;
using FluentValidation;

namespace EventSalesBackend.Validators.Hosts;

public class CreateHostRequestValidator : AbstractValidator<CreateHostRequest>
{
    public CreateHostRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required")
            .Length(2, 20).WithMessage("First Name must be between 2 and 20 characters")
            .Matches("[a-zA-Z]+$").WithMessage("First Name must contain only letters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required")
            .Length(2, 20).WithMessage("Last Name must be between 2 and 20 characters")
            .Matches("[a-zA-Z]+$").WithMessage("Last Name must contain only letters");
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Date of Birth is required")
            .Must(dob =>
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - dob.Year;

                // If birthday hasn't occurred yet this year, subtract 1
                if (dob > today.AddYears(-age)) age--;

                return age >= 18 && age <= 99;
            })
            .WithMessage("Age must be between 18 and 99 years");
    }
}