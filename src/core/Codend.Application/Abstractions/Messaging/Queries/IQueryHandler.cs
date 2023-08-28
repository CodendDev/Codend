using FluentResults;
using MediatR;

namespace Codend.Application.Abstractions.Messaging.Queries;

/// <summary>
/// Interface of query handler
/// </summary>
/// <typeparam name="TQuery">Type of handled command</typeparam>
/// <typeparam name="TResponse">Result response type</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}