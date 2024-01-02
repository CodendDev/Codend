using Codend.Domain.Core.Abstractions;
using Codend.Domain.ValueObjects.Abstractions;
using FluentResults;

namespace Codend.Domain.Core.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static partial class DomainErrors
{
    /// <summary>
    /// Domain error base class.
    /// </summary>
    public abstract class DomainError : ApiError
    {
        /// <inheritdoc />
        protected DomainError(string errorCode, string message) : base(errorCode, message)
        {
        }
    }

    /// <summary>
    /// General domain errors.
    /// </summary>
    public static class General
    {
        public class ServerError : DomainError
        {
            public ServerError() : base("General.ServerError", $"Unexpected server error.")
            {
            }
        }

        public class DomainNotFound : DomainError
        {
            public DomainNotFound(string domainName)
                : base($"General.DomainNotFound.{domainName}", $"{domainName} was not found.")
            {
            }

            /// <summary>
            /// Creates a failed <see cref="Result"/> with <see cref="DomainErrors.General.DomainNotFound"/> error
            /// with nameof(<typeparamref name="T"/>) as domain name.
            /// </summary>
            /// <typeparam name="T">Domain which was not found</typeparam>
            /// <returns><see cref="Result"/> with <see cref="DomainErrors.General.DomainNotFound"/> error.</returns>
            public static Result Fail<T>()
                where T : IEntity
                => Result.Fail(new DomainNotFound(typeof(T).Name));
        }

        public class UnspecifiedBadRequest : DomainError
        {
            public UnspecifiedBadRequest()
                : base("General.UnspecifiedBadRequest",
                    "Bad request with unspecified errors. Check if values are valid for types.")
            {
            }
        }
    }

    /// <summary>
    /// Common errors for string value objects.
    /// </summary>
    public static class StringValueObject
    {
        public class NullOrEmpty<T> : DomainError
            where T : ValueObjects.Primitives.StringValueObject
        {
            public NullOrEmpty() : base($"StringValueObject.NullOrEmpty.{typeof(T).Name}",
                $"Field {typeof(T).Name} cannot be null nor empty.")
            {
            }
        }

        public class TooLong<T> : DomainError
            where T : IStringMaxLengthValueObject
        {
            public TooLong() : base($"StringValueObject.TooLong.{typeof(T).Name}",
                $"Field {typeof(T).Name} is longer than allowed {T.MaxLength}.")
            {
            }
        }
    }
}