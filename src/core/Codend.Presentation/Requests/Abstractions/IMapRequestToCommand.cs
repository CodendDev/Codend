using Codend.Application.Core.Abstractions.Messaging.Commands;

namespace Codend.Presentation.Requests.Abstractions;

/// <summary>
/// Interface allows object to be mapped to <see cref="ICommand{TResponse}"/>.
/// </summary>
/// <typeparam name="TCommand">Command.</typeparam>
/// <typeparam name="TCommandResponse">Command response type.</typeparam>
public interface IMapRequestToCommand<out TCommand, TCommandResponse> where TCommand : ICommand<TCommandResponse>
{
    /// <summary>
    /// Maps object to <see cref="TCommand"/>.
    /// </summary>
    /// <returns>Command.</returns>
    public TCommand MapToCommand();
}