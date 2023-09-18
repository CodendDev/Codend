using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

public class EpicName : StringValueObject, IStringValueObject<EpicName>
{
    private EpicName(string value) : base(value)
    {
    }

    public static Result<EpicName> Create(string value)
    {
        return Result
            .Ok(new EpicName(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(EpicName)))
            .Ensure(() => value.Length < MaxLength, new TooLong(nameof(EpicName), MaxLength));
    }

    public static int MaxLength => 100;
}