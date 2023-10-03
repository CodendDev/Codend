using Codend.Domain.Core.Abstractions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;

namespace Codend.Application.Extensions;

/// <summary>
/// Contains extension methods for fluent validations.
/// </summary>
internal static class FluentValidationExtensions
{
    /// <summary>
    /// Specifies a custom error to use if validation fails.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The property being validated.</typeparam>
    /// <param name="rule">The current rule.</param>
    /// <param name="error">The error to use.</param>
    /// <returns>The same rule builder.</returns>
    internal static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, ValidationError error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "The error is required");
        }

        return rule.WithErrorCode(error.ErrorCode).WithMessage(error.Message);
    }

    /// <summary>
    /// Validates that provided guid is not default guid.
    /// </summary>
    /// <param name="rule">The current rule.</param>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The nullable <see cref="IEntityId{TKey}"/> being validated.</typeparam>
    /// <returns>The same rule builder.</returns>
    public static IRuleBuilderOptions<T, TProperty> MustNotBeDefaultGuid<T, TProperty>(
        this IRuleBuilder<T, TProperty> rule) where TProperty : IEntityId<Guid>?
    {
        return rule.Must((rootObject, property, context) =>
        {
            if (property == null)
            {
                return true;
            }

            return property.Value != Guid.Empty;
        });
    }
}