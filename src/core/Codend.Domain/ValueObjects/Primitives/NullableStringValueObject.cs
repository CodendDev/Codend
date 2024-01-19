using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects.Abstractions;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects.Primitives;

// Without this class automapper doesn't work 💀
public abstract class NullableStringValueObject : ValueObject
{
    /// <summary>
    /// String value.
    /// </summary>
    public string? Value { get; }

    protected NullableStringValueObject(string? value)
    {
        Value = value;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        if (Value != null) yield return Value;
    }
}

/// <summary>
/// Generic nullable string value object.
/// </summary>
public abstract class NullableStringValueObject<TSelf> : NullableStringValueObject
    where TSelf : NullableStringValueObject<TSelf>, IStringMaxLengthValueObject
{
    protected NullableStringValueObject(string? value) : base(value)
    {
    }

    protected static Result<TSelf> Validate(TSelf value)
    {
        return Result
            .Ok(value)
            .Ensure(() => value.Value is null || value.Value.Length < TSelf.MaxLength, new TooLong<TSelf>());
    }
}