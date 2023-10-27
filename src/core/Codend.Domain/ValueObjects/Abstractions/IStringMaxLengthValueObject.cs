using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;

namespace Codend.Domain.ValueObjects.Abstractions;

public interface IStringMaxLengthValueObject
{
    static abstract int MaxLength { get; }
}

public static class StringMaxLengthValueObjectExtensions
{
    public static Result<T> EnsureStringMaxLength<T>(
        this Result<T> result,
        string value,
        DomainErrors.DomainError error)
        where T : IStringMaxLengthValueObject
    {
        return result.Ensure(() => value.Length < T.MaxLength, error);
    }
}
