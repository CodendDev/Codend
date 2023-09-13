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
    
    public static class StoryDescription
    {
        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("StoryDescription.NullOrEmpty", "Story description can't be null or empty.")
            {
            }
        }

        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("StoryDescription.DescriptionTooLong", "Story description is too long.")
            {
            }
        }
    }

    public static class StoryName
    {
        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("StoryName.NullOrEmpty", "Story name can't be null or empty.")
            {
            }
        }

        public class NameTooLong : DomainError
        {
            public NameTooLong()
                : base("StoryName.DescriptionTooLong", "Story name is too long.")
            {
            }
        }
    }
}