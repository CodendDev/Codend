using Codend.Domain.Core.Errors;
using FluentResults;

namespace Codend.Contracts;

/// <summary>
/// Represents an API errors list response.
/// </summary>
public sealed class ApiErrorsResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiErrorsResponse"/> class.
    /// </summary>
    /// <param name="errors">The enumerable collection of <see cref="IError"/>.</param>
    public ApiErrorsResponse(IReadOnlyCollection<IError> errors) =>
        Errors = errors.Select(x => x.MapToApiErrorResponse()).ToList();

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiErrorsResponse"/> class.
    /// </summary>
    /// <param name="errors">The enumerable collection of <see cref="ApiError"/>.</param>
    public ApiErrorsResponse(IReadOnlyCollection<ApiErrorResponse> errors) => Errors = errors;

    /// <summary>
    /// Gets the errors.
    /// </summary>
    public IReadOnlyCollection<ApiErrorResponse> Errors { get; }
}

/// <summary>
/// Mappers for <see cref="ApiErrorsResponse"/>.
/// </summary>
public static class ApiErrorsResponseMappers
{
    /// <summary>
    /// Converts <see cref="IResultBase"/> reasons to ApiErrorsResponse.
    /// </summary>
    /// <param name="result">Result to be converted.</param>
    /// <returns>New <see cref="ApiErrorsResponse"/> with all reasons having only ErrorCode and Message values.</returns>
    public static ApiErrorsResponse MapToApiErrorsResponse(this IResultBase result) => new(
        result.Reasons.Select(x => x.MapToApiErrorResponse()).ToList());
}