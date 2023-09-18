using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Stories.Commands.UpdateStory;

/// <summary>
/// Validates <see cref="UpdateStoryCommand"/>.
/// </summary>
public class UpdateStoryCommandValidator : AbstractValidator<UpdateStoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStoryCommandValidator"/> class.
    /// </summary>
    public UpdateStoryCommandValidator()
    {
        RuleFor(x => x.StoryId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(UpdateStoryCommand.StoryId)));

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateStoryCommand.Name)))
                .MaximumLength(StoryName.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateStoryCommand.Name), StoryName.MaxLength));
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithError(new PropertyNullOrEmpty(nameof(UpdateStoryCommand.Description)))
                .MaximumLength(StoryDescription.MaxLength)
                .WithError(new StringPropertyTooLong(nameof(UpdateStoryCommand.Description), StoryDescription.MaxLength));
        });
    }
}