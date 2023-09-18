using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

public class StoryName : StringValueObject, IStringValueObject<StoryName>
{
    private StoryName(string value) : base(value)
    {
    }

    public static Result<StoryName> Create(string value)
    {
        return Result
            .Ok(new StoryName(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(StoryName)))
            .Ensure(() => value.Length < MaxLength, new TooLong<StoryName>());
    }

    public static int MaxLength => 100;
}