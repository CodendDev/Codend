using Codend.Contracts.Abstractions;
using FluentResults;

namespace Codend.Application.Core;

internal static class ShouldUpdateExtensions
{
    internal static Result HandleUpdate<T>(this IShouldUpdate<T> shouldUpdate, Func<T, Result<T>> updateHandler)
    {
        if (shouldUpdate.ShouldUpdate)
        {
            return updateHandler(shouldUpdate.Value!).ToResult();
        }

        return Result.Ok();
    }

    internal static Result HandleUpdateWithResult<T, TResult>(this IShouldUpdate<T> shouldUpdate,
        Func<T, Result<TResult>> updateHandler)
    {
        if (shouldUpdate.ShouldUpdate)
        {
            return updateHandler(shouldUpdate.Value!).ToResult();
        }

        return Result.Ok();
    }
}