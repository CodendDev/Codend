using Codend.Application.Extensions;
using Codend.Domain.Core.Errors;
using FluentValidation;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

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
            .WithError(new NullOrEmpty(nameof(LoginCommand.Email)));
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(new NullOrEmpty(nameof(LoginCommand.Password)));
    }
}