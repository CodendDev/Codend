using FluentResults;

namespace Codend.Contracts;

public sealed record ApiErrorResponse(string ErrorCode, string Message);

/// <summary>
/// Mappers for <see cref="ApiErrorResponse"/>.
/// </summary>
public static class ApiErrorResponseMappers
{
    /// <summary>
    /// Converts <see cref="IReason"/> to ApiErrorResponse.
    /// </summary>
    /// <param name="reason">Reason to be converted.</param>
    /// <returns>New <see cref="ApiErrorResponse"/> with mapped ErrorCode and Message values.</returns>
    public static ApiErrorResponse MapToApiErrorResponse(this IReason reason) =>
        new ApiErrorResponse(reason.Metadata["ErrorCode"].ToString() ?? string.Empty, reason.Message);

}