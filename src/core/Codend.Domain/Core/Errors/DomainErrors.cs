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
        }
    }

    /// <summary>
    /// Common errors for string value objects.
    /// </summary>
    public static class StringValueObject
    {
        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty(string fieldName) : base($"StringValueObject.NullOrEmpty.{fieldName}",
                $"Field {fieldName} cannot be null nor empty.")
            {
            }
        }

        public class TooLong : DomainError
        {
            public TooLong(string fieldName, int maxLength) : base($"StringValueObject.TooLong.{fieldName}",
                $"Field {fieldName} is longer than allowed {maxLength}.")
            {
            }
        }
    }
}