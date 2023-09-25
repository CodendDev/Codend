using Codend.Contracts.Requests;
using FluentResults;

namespace Codend.Application.Core;

internal static class ShouldUpdateExtensions
{
    internal static Result HandleUpdate<T>(this ShouldUpdateBinder<T> shouldUpdate, Func<T, Result<T>> updateHandler)
    {
        if (shouldUpdate.ShouldUpdate)
        {
            return updateHandler(shouldUpdate.Value!).ToResult();
        }

        return Result.Ok();
    }

    internal static Result HandleUpdateWithResult<T, TResult>(this ShouldUpdateBinder<T> shouldUpdate,
        Func<T, Result<TResult>> updateHandler)
    {
        if (shouldUpdate.ShouldUpdate)
        {
            return updateHandler(shouldUpdate.Value!).ToResult();
        }

        return Result.Ok();
    }
}