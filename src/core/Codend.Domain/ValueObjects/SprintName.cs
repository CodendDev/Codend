using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

public class SprintName : StringValueObject<SprintName>, IStringMaxLengthValueObject
{
    private SprintName(string value) : base(value)
    {
    }

    public static Result<SprintName> Create(string value) => Validate(new SprintName(value));

    public static int MaxLength => 100;
}