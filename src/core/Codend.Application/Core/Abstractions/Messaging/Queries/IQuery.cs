using FluentResults;
using MediatR;

namespace Codend.Application.Core.Abstractions.Messaging.Queries;

/// <inheritdoc />
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}