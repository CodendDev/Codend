using Codend.Contracts;
using Codend.Presentation.Infrastructure;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Presentation.Extensions;

/// <summary>
/// Extensions methods for fluent command/query response resolution and authorization.
/// </summary>
internal static class ResolverExtensions
{
    /// <summary>
    /// Executes function and returns it result.
    /// </summary>
    /// <typeparam name="TIn">The result type.</typeparam>
    /// <typeparam name="TOut">The output result type.</typeparam>
    /// <param name="resolver">The resolver.</param>
    /// <param name="func">Function to execute.</param>
    /// <returns>
    /// The success result with the bound value if the current result is a success result, otherwise a failure result.
    /// </returns>
    public static async Task<Result<TOut>> Execute<TIn, TOut>(this Resolver<TIn> resolver,
        Func<TIn, Task<Result<TOut>>> func) =>
        await func(resolver.Request);

    /// <inheritdoc cref="Execute{TIn,TOut}"/>
    /// <remarks>Where TOut is <see cref="Result"/></remarks>
    public static async Task<Result> Execute<TIn>(this Resolver<TIn> resolver,
        Func<TIn, Task<Result>> func) =>
        await func(resolver.Request);

    /// <summary>
    /// Resolves command/query response. Returns succes or error response based on result.
    /// </summary>
    /// <param name="responseTask">Async task of the response to be resolved.</param>
    /// <param name="controller">Controller instance.</param>
    /// <returns>
    /// 200 OK on success.
    /// 404 NotFound when <see cref="DomainNotFound"/> present in result errors.
    /// 400 BadRequest when other errors present.
    /// </returns>
    /// <inheritdoc cref="ResolveResponse"/>
    internal static async Task<IActionResult> ResolveResponse<T>(
        this Task<Result<T>> responseTask,
        ApiController controller)
    {
        var response = await responseTask;
        if (response.IsSuccess)
        {
            return controller.Ok(response.Value);
        }

        if (response.HasError<DomainNotFound>())
        {
            return controller.NotFound();
        }

        return controller.BadRequest(response.MapToApiErrorsResponse());
    }

    /// <inheritdoc cref="ResolveResponse{T}"/>
    /// <remarks>Executed when there is no return type/value.</remarks>
    /// <returns>
    /// 204 NoContent on success.
    /// 404 NotFound when <see cref="DomainNotFound"/> present in result errors.
    /// 400 BadRequest when other errors present.
    /// </returns>
    internal static async Task<IActionResult> ResolveResponse(
        this Task<Result> responseTask,
        ApiController controller)
    {
        var response = await responseTask;
        if (response.IsSuccess)
        {
            return controller.NoContent();
        }

        if (response.HasError<DomainNotFound>())
        {
            return controller.NotFound();
        }

        return controller.BadRequest(response.MapToApiErrorsResponse());
    }
}