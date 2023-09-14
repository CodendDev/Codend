using FluentResults;

namespace Codend.Domain.ValueObjects.Abstractions;

public interface INullableStringValueObject
{
    static abstract int MaxLength { get; }
    public string? Value { get; }
}

public interface INullableStringValueObject<T> : INullableStringValueObject
{
    static abstract Result<T> Create(string? value);
}