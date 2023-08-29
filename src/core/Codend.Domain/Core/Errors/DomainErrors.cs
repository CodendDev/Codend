using FluentResults;

namespace Codend.Domain.Core.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static class DomainErrors
{
    public abstract class DomainError : Error
    {
        protected DomainError(string errorCode, string message)
        {
            Metadata.Add("ErrorCode", errorCode);
            Metadata.Add("Message", message);
        }
    }
}