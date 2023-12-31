﻿using FluentResults;
using MediatR;

namespace Codend.Application.Core.Abstractions.Messaging.Commands;

/// <summary>
/// Command handler without response type.
/// </summary>
/// <typeparam name="TCommand">Type of handled command.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

/// <summary>
/// Command handler with response type.
/// </summary>
/// <typeparam name="TCommand">Type of handled command.</typeparam>
/// <typeparam name="TResponse">Response type. Response will be wrapped in <see cref="Result"/>.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}