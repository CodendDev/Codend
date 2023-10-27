using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Projects.Commands.UpdateIsFavouriteFlag;

/// <summary>
/// Validates <see cref="UpdateProjectIsFavouriteFlagCommand"/>.
/// </summary>
public class UpdateProjectIsFavouriteFlagCommandValidator : AbstractValidator<UpdateProjectIsFavouriteFlagCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectIsFavouriteFlagCommandValidator"/> class.
    /// </summary>
    public UpdateProjectIsFavouriteFlagCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectIsFavouriteFlagCommand.ProjectId)));

        RuleFor(x => x.IsFavourite)
            .NotNull()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateProjectIsFavouriteFlagCommand.IsFavourite)));
    }
}