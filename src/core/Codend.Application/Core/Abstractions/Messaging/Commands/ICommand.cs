using FluentResults;
using MediatR;

namespace Codend.Application.Core.Abstractions.Messaging.Commands;

/// <summary>
/// Mediator command interface.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Mediator command with response interface.
/// </summary>
/// <typeparam name="TResponse">Response class.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}