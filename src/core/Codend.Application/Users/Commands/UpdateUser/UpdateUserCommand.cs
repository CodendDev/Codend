using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Core.Abstractions.Services;
using Codend.Domain.Entities;
using FluentResults;

namespace Codend.Application.Users.Commands.UpdateUser;

/// <summary>
/// Command for updating user data.
/// </summary>
/// <param name="FirstName">New user first name.</param>
/// <param name="LastName">New user last name.</param>
/// <param name="ImageUrl">New user ImageUrl.</param>
public sealed record UpdateUserCommand
(
    string FirstName,
    string LastName,
    string ImageUrl
) : ICommand;

/// <summary>
/// <see cref="UpdateUserCommand"/> handler.
/// </summary>
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;
    private readonly IHttpContextProvider _contextProvider;

    /// <summary>
    /// Constructs <see cref="UpdateUserCommandHandler"/>.
    /// </summary>
    public UpdateUserCommandHandler(IUserService userService, IHttpContextProvider contextProvider)
    {
        _userService = userService;
        _contextProvider = contextProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var result = await _userService.UpdateUserAsync(userId, request);
        return result.IsFailed ? result : Result.Ok();
    }
}