using System.Text.RegularExpressions;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Authentication.Register;

/// <summary>
/// Validator for <see cref="RegisterCommand"/>
/// </summary>
public partial class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RegisterCommand.Email)))
            .MaximumLength(IAuthService.MaxEmailLength)
            .WithError(new StringPropertyTooLong(nameof(RegisterCommand.Email), IAuthService.MaxEmailLength))
            .EmailAddress()
            .WithError(new EmailAddress.NotValid());

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RegisterCommand.Password)))
            .MaximumLength(IAuthService.MaxPasswordLength)
            .WithError(new StringPropertyTooLong(nameof(RegisterCommand.Password), IAuthService.MaxPasswordLength))
            .MinimumLength(IAuthService.MinPasswordLength)
            .WithError(new Password.TooShort())
            .Must(ContainLowercaseLetter)
            .WithError(new Password.NotContainLowercaseLetter())
            .Must(ContainUppercaseLetter)
            .WithError(new Password.NotContainUppercaseLetter())
            .Must(ContainDigit)
            .WithError(new Password.NotContainDigit())
            .Must(ContainCustomChar)
            .WithError(new Password.NotContainCustomChar());

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RegisterCommand.FirstName)))
            .MaximumLength(IAuthService.MaxFirstNameLength)
            .WithError(new StringPropertyTooLong(nameof(RegisterCommand.FirstName), IAuthService.MaxFirstNameLength));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RegisterCommand.LastName)))
            .MaximumLength(IAuthService.MaxLastNameLength)
            .WithError(new StringPropertyTooLong(nameof(RegisterCommand.LastName), IAuthService.MaxLastNameLength));

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RegisterCommand.ImageUrl)));
    }

    private static bool ContainLowercaseLetter(string password)
    {
        return LowercaseRegex().IsMatch(password);
    }

    private static bool ContainUppercaseLetter(string password)
    {
        return UppercaseRegex().IsMatch(password);
    }

    private static bool ContainDigit(string password)
    {
        return DigitRegex().IsMatch(password);
    }

    private static bool ContainCustomChar(string password)
    {
        return CustomCharRegex().IsMatch(password);
    }


    [GeneratedRegex("[a-z]")]
    private static partial Regex LowercaseRegex();

    [GeneratedRegex("[A-Z]")]
    private static partial Regex UppercaseRegex();

    [GeneratedRegex("\\d")]
    private static partial Regex DigitRegex();

    [GeneratedRegex("[@$#!%*?&]")]
    private static partial Regex CustomCharRegex();
}