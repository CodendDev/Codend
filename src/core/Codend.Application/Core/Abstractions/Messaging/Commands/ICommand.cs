using FluentResults;
using MediatR;

namespace Codend.Application.Core.Abstractions.Messaging.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}