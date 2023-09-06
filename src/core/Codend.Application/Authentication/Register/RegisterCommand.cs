using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Authentication;
using FluentResults;

namespace Codend.Application.Authentication.Register;

/// <summary>
/// Register command.
/// </summary>
/// <param name="Email">User email.</param>
/// <param name="Password">User password.</param>
/// <param name="FirstName">User first name.</param>
/// <param name="LastName">User last name.</param>
public sealed record RegisterCommand(string Email, string Password, string FirstName, string LastName) : ICommand<TokenResponse>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, TokenResponse>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<TokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(new TokenResponse(result.Value));
    }
}