using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

public class EpicDescription : StringValueObject, IStringValueObject<EpicDescription>
{
    private EpicDescription(string value) : base(value)
    {
    }

    public static Result<EpicDescription> Create(string value)
    {
        return Result
            .Ok(new EpicDescription(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(EpicDescription)))
            .Ensure(() => value.Length < MaxLength, new TooLong<EpicDescription>());
    }

    public static int MaxLength => 3000;
}