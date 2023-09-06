using FluentResults;
using MediatR;

namespace Codend.Application.Core.Abstractions.Messaging.Commands;

/// <summary>
/// Interface of command handler without response type
/// </summary>
/// <typeparam name="TCommand">Type of handled command</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

/// <summary>
/// Interface of command handler
/// </summary>
/// <typeparam name="TCommand">Type of handled command</typeparam>
/// <typeparam name="TResponse">Result response type</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}