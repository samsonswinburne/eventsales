using EventSalesBackend.Models.DTOs.Request.Tickets;
using FluentValidation;

namespace EventSalesBackend.Validators.Tickets;

public class CreateTicketsRequestValidator : AbstractValidator<CreateTicketsRequest>
{
    public CreateTicketsRequestValidator()
    {
        
    }
}