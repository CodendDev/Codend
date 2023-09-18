using FluentResults;

namespace Codend.Domain.ValueObjects.Abstractions;

public interface INullableStringValueObject : IStringMaxLengthValueObject
{
    public string? Value { get; }
}

public interface INullableStringValueObject<T> : INullableStringValueObject
{
    static abstract Result<T> Create(string? value);
}