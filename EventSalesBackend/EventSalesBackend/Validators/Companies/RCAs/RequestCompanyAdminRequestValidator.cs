using EventSalesBackend.Models;
using EventSalesBackend.Models.DTOs.Request.Companies;
using FluentValidation;

namespace EventSalesBackend.Validators.Companies.RCAs;

public class RequestCompanyAdminRequestValidator : AbstractValidator<RequestCompanyAdminRequest>
{
    public RequestCompanyAdminRequestValidator()
    {
        RuleFor(x => x.AdminRequestReceiverEmail)
            .NotEmpty()
            .EmailAddress();
    }
}