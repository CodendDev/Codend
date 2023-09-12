using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Authentication;
using FluentResults;

namespace Codend.Application.Authentication.Login;

/// <summary>
/// Login command.
/// </summary>
/// <param name="Email">User email.</param>
/// <param name="Password">User password.</param>
public sealed record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, TokenResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(new TokenResponse(result.Value));
    }
}