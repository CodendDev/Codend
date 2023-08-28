using FluentResults;
using MediatR;

namespace Codend.Application.Abstractions.Messaging.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}