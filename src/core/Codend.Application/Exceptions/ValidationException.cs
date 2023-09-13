using Codend.Domain.Core.Errors;
using FluentValidation.Results;

namespace Codend.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when a validation fails.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="failures">The collection of validation failures.</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures has occurred.") =>
        Errors = failures
            .Distinct()
            .Select(failure => new ApiError(failure.ErrorCode, failure.ErrorMessage))
            .ToList();

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IReadOnlyCollection<ApiError> Errors { get; }
}