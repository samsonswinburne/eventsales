using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EventSalesBackend.Extensions;

public static class ControllerExtensions
{
    public static async Task<ActionResult?> ValidateAsync<T>(
        this ControllerBase controller,
        T model,
        IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(model);
            
        if (!validationResult.IsValid)
        {
            return controller.BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new
                {
                    property = e.PropertyName,
                    message = e.ErrorMessage
                })
            });
        }

        return null; // Validation passed
    }
}