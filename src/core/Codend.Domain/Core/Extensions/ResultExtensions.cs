using Codend.Domain.Core.Errors;
using FluentResults;

namespace Codend.Domain.Core.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Checks if given predicate is assured. If it's not assured result will be a failure.
    /// </summary>
    /// <param name="result">Result instance.</param>
    /// <param name="predicate">Predicate function.</param>
    /// <param name="error">Error which will be added to result if predicate isn't assured.</param>
    /// <returns>Result with added error if predicate was assured.</returns>
    public static Result<T> Ensure<T>(this Result<T> result, Func<bool> predicate, DomainErrors.DomainError error)
    {
        try
        {
            if (predicate())
            {
                return result;
            }
        }
        catch (Exception)
        {
            return result.WithError(error);
        }

        return result.WithError(error);
    }
}