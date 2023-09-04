using Codend.Domain.Core.Primitives;

namespace Codend.Domain.ValueObjects.Primitives;

/// <summary>
/// String value object.
/// </summary>
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