using FluentResults;

namespace Codend.Domain.ValueObjects.Abstractions;

public interface IStringValueObject : IStringMaxLengthValueObject
{
    public string Value { get; }
}

public interface IStringValueObject<T> : IStringValueObject
{
    static abstract Result<T> Create(string value);
}