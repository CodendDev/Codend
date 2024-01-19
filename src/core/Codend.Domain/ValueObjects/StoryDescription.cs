using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

public class StoryDescription : StringValueObject<StoryDescription>, IStringMaxLengthValueObject
{
    private StoryDescription(string value) : base(value)
    {
    }

    public static Result<StoryDescription> Create(string value) => Validate(new StoryDescription(value));

    public static int MaxLength => 3000;
}