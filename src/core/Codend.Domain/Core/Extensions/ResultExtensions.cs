using Codend.Domain.Core.Errors;
using FluentResults;

namespace Codend.Domain.Core.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Checks if given predicate is assured. If it's not assured result will be a failure. 
    /// </summary>
    /// <remarks>
    /// Use only for errors with parameters.
    /// </remarks>
    /// <param name="result">Result instance.</param>
    /// <param name="predicate">Predicate function.</param>
    /// <param name="error">Error which will be added to result if predicate isn't assured.</param>
    /// <returns>Result with added error if predicate was not assured.</returns>
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
    /// <typeparam name="T">Type of the entity we are ensuring.</typeparam>
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

    /// <summary>
    /// Merges given result of type <typeparamref name="T"/> with other results <see cref="Result"/>
    /// </summary>
    /// <param name="result"> instance. </param>
    /// <param name="results">Results to be merged into instance.</param>
    /// <typeparam name="T">Result type.</typeparam>
    /// <returns><see cref="Result"/> of type <typeparamref name="T"/> with merged errors and successes.</returns>
    public static Result<T> MergeReasons<T>(this Result<T> result, params Result[] results)
    {
        return result.WithReasons(results.Merge().Reasons);
    }

    // TODO come up with cool name xd
    /// <summary>
    /// Helper method for creating <see cref="Result{T}"/>, allows to pass custom null value handler.
    /// If <paramref name="value"/> is null method returns <paramref name="nullHandler"/> result.
    /// Otherwise delegate <paramref name="getResult"/> is called returns its result.
    /// </summary>
    /// <param name="value">
    /// Object which will be passed to a <paramref name="getResult"/> if it's not null.
    /// </param>
    /// <param name="getResult">
    /// Function which will be called to get a result.
    /// </param>
    /// <param name="nullHandler">
    /// Function which will be called if <paramref name="value"/> is null.
    /// </param>
    /// <returns>
    /// If value is null <paramref name="nullHandler"/> result.
    /// If value is not null <paramref name="getResult"/> result.
    /// </returns>
    public static Result<TOut> GetResultFromDelegate<TIn, TOut>(
        this TIn? value,
        Func<TIn, Result<TOut>> getResult,
        Func<Result> nullHandler
    )
        where TIn : class
    {
        if (value is null)
        {
            return nullHandler();
        }

        var result = getResult(value);
        return result;
    }
}