using FluentValidation.Results;

namespace Codend.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when a validation fails because of not found or unauthorized error.
/// </summary>
public sealed class NotFoundValidationException : ValidationException
{
    /// <inheritdoc />
    public NotFoundValidationException(IEnumerable<ValidationFailure> failures) : base(failures)
    {
    }
}