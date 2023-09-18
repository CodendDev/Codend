using Codend.Application.Extensions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;

namespace Codend.Application.Stories.Commands.DeleteStory;

/// <summary>
/// Validates <see cref="DeleteStoryCommand"/>.
/// </summary>
public class DeleteStoryCommandValidator : AbstractValidator<DeleteStoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteStoryCommandValidator"/> class.
    /// </summary>
    public DeleteStoryCommandValidator()
    {
        RuleFor(x => x.StoryId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(DeleteStoryCommand.StoryId)));
    }
}