using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;

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

public static class StringValueObjectExtensions
{
    public static Result<T> EnsureStringNotNullOrEmpty<T>(
        this Result<T> result,
        string? value,
        DomainErrors.DomainError error)
    {
        return result.Ensure(() => !string.IsNullOrEmpty(value), error);
    }
}