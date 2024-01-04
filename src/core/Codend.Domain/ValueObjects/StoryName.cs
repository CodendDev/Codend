using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

public class StoryName : StringValueObject<StoryName>, IStringMaxLengthValueObject
{
    private StoryName(string value) : base(value)
    {
    }

    public static Result<StoryName> Create(string value) => Validate(new StoryName(value));

    public static int MaxLength => 100;
}