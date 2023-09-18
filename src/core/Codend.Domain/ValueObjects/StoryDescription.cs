using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

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
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(StoryDescription)))
            .Ensure(() => value.Length < MaxLength, new TooLong(nameof(StoryDescription), MaxLength));
    }

    public static int MaxLength => 3000;
}