using Codend.Domain.Core.Primitives;

namespace Codend.Domain.ValueObjects.Primitives;

/// <summary>
/// String value object.
/// </summary>
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