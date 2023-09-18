using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Application.Stories.Commands.CreateStory;

/// <summary>
/// Validates <see cref="CreateStoryCommand"/>.
/// </summary>
public class CreateStoryCommandValidator : AbstractValidator<CreateStoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStoryCommandValidator"/> class.
    /// </summary>
    public CreateStoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new NullOrEmpty(nameof(CreateStoryCommand.Name)))
            .MaximumLength(StoryName.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateStoryCommand.Name), StoryName.MaxLength));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(new NullOrEmpty(nameof(CreateStoryCommand.Description)))
            .MaximumLength(StoryDescription.MaxLength)
            .WithError(new StringPropertyTooLong(nameof(CreateStoryCommand.Description), StoryDescription.MaxLength));

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(CreateStoryCommand.ProjectId)));
    }
}