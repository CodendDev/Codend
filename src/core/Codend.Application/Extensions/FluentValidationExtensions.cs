using Codend.Domain.Core.Errors;
using FluentValidation;

namespace Codend.Application.Extensions;

/// <summary>
/// Contains extension methods for fluent validations.
/// </summary>
public static class FluentValidationExtensions
{
    /// <summary>
    /// Specifies a custom error to use if validation fails.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The property being validated.</typeparam>
    /// <param name="rule">The current rule.</param>
    /// <param name="error">The error to use.</param>
    /// <returns>The same rule builder.</returns>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, ApiError error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "The error is required");
        }

        return rule.WithErrorCode(error.ErrorCode).WithMessage(error.Message);
    }
}