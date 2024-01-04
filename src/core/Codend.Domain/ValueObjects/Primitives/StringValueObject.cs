using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects.Abstractions;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects.Primitives;

// Without this class automapper doesn't work 💀
public abstract class StringValueObject : ValueObject
{
    /// <summary>
    /// String value.
    /// </summary>
    public string Value { get; }

    protected StringValueObject(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

/// <summary>
/// String value object.
/// </summary>
public abstract class StringValueObject<TSelf> : StringValueObject
    where TSelf : StringValueObject<TSelf>, IStringMaxLengthValueObject
{
    protected StringValueObject(string value) : base(value)
    {
    }

    protected static Result<TSelf> Validate(TSelf value)
    {
        return Result
            .Ok(value)
            .Ensure(() => !string.IsNullOrEmpty(value.Value), new NullOrEmpty<TSelf>())
            .Ensure(() => value.Value.Length < TSelf.MaxLength, new TooLong<TSelf>());
    }
}