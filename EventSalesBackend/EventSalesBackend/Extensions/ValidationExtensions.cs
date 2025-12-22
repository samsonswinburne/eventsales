using FluentValidation.Results;

namespace EventSalesBackend.Extensions;

public static class ValidationExtensions
{
    public static object ToErrorResponse(this ValidationResult validationResult)
    {
        return new
        {
            errors = validationResult.Errors.Select(e => new
                {
                    field = ToCamelCase(e.PropertyName),
                    error = e.ErrorMessage ?? "error"
                }
            )
        };
    }

    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }
}