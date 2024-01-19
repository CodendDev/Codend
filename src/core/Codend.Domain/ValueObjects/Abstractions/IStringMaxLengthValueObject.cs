namespace Codend.Domain.ValueObjects.Abstractions;

public interface IStringMaxLengthValueObject
{
    static abstract int MaxLength { get; }
}