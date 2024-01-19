using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

public class EpicName : StringValueObject<EpicName>, IStringMaxLengthValueObject
{
    private EpicName(string value) : base(value)
    {
    }

    public static Result<EpicName> Create(string value) => Validate(new EpicName(value));

    public static int MaxLength => 100;
}