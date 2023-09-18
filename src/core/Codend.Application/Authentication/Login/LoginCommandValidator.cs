using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Authentication.Login;

/// <summary>
/// Validator for <see cref="LoginCommand"/>
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(LoginCommand.Email)));
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(LoginCommand.Password)));
    }
}