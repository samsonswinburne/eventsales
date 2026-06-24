using EventSalesBackend.Models.DTOs.Request.Tickets;
using FluentValidation;

namespace EventSalesBackend.Validators.Tickets;

public class CreateTicketsResponseValidator : AbstractValidator<CreateTicketsResponse>
{
    public CreateTicketsResponseValidator()
    {
        
    }
}