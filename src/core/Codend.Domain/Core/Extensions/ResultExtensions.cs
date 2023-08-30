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
    /// <returns>Result with added error if predicate was not assured.</returns>
    [Obsolete("forgor ☠", false)]
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

    /// <summary>
    /// Checks if given predicate is assured. If it's not assured result will be a failure.
    /// </summary>
    /// <param name="result"><see cref="Result"/> instance.</param>
    /// <param name="predicate">Predicate function.</param>
    /// <typeparam name="TError"><see cref="DomainErrors.DomainError"/> which will be added to result if predicate isn't assured.</typeparam>
    /// <returns><see cref="Result"/> with added <see cref="DomainErrors.DomainError"/> if predicate was not assured.</returns>
    public static Result<T> Ensure<T, TError>(this Result<T> result, Func<bool> predicate)
        where TError : DomainErrors.DomainError, new()
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
            return result.WithError(new TError());
        }

        return result.WithError(new TError());
    }
}