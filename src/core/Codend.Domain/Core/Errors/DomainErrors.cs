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
            public ServerError() : base("General.ServerError", "Unexpected server error.")
            {
            }
        }
    }
}