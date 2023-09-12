using FluentResults;

namespace Codend.Domain.ValueObjects.Abstractions;

public interface IStringValueObject
{
    static abstract int MaxLength { get; }
    public string Value { get; }
}

public interface IStringValueObject<T> : IStringValueObject
{
    static abstract Result<T> Create(string value);
}