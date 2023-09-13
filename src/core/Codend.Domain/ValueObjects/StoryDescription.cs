using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StoryDescription;

namespace Codend.Domain.ValueObjects;

public class StoryDescription : StringValueObject, IStringValueObject<StoryDescription>
{
    private StoryDescription(string value) : base(value)
    {
    }

    public static Result<StoryDescription> Create(string value)
    {
        return Result
            .Ok(new StoryDescription(value))
            .Ensure<StoryDescription, NullOrEmpty>(() => !string.IsNullOrEmpty(value))
            .Ensure<StoryDescription, DescriptionTooLong>(() => value.Length < MaxLength);
    }

    public static int MaxLength => 3000;
}