using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

public class SprintName : StringValueObject, IStringValueObject<SprintName>
{
    public SprintName(string value) : base(value)
    {
    }

    public static Result<SprintName> Create(string value)
    {
        return Result
            .Ok(new SprintName(value))
            .EnsureStringNotNullOrEmpty(value, new NullOrEmpty<SprintName>())
            .EnsureStringMaxLength(value, new TooLong<SprintName>());
    }

    public static int MaxLength => 100;
}
