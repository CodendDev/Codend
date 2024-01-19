using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

public class EpicDescription : StringValueObject<EpicDescription>, IStringMaxLengthValueObject
{
    private EpicDescription(string value) : base(value)
    {
    }

    public static Result<EpicDescription> Create(string value) => Validate(new EpicDescription(value));

    public static int MaxLength => 3000;
}