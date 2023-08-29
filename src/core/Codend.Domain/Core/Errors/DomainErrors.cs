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

    /// <summary>
    /// ProjectTask domain errors.
    /// </summary>
    public static class ProjectTaskName
    {
        public class NameTooLong : DomainError
        {
            public NameTooLong() : base("ProjectTaskName.NameTooLong", "ProjectTask name is too long.")
            {
            }
        }

        public class NameNullOrEmpty : DomainError
        {
            public NameNullOrEmpty() : base("ProjectTaskName.NameNullOrEmpty", "ProjectTask name is null or empty.")
            {
            }
        }
    }

    public static class ProjectTaskDescription
    {
        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong() : base("ProjectTaskDescription.DescriptionTooLong",
                "ProjectTask description is too long.")
            {
            }
        }
    }

    /// <summary>
    /// ProjectTaskStatus domain errors
    /// </summary>
    public static class ProjectTaskStatus
    {
        public class NameTooLong : DomainError
        {
            public NameTooLong() : base("ProjectTaskStatus.NameTooLong",
                "ProjectTask status is too long.")
            {
            }
        }

        public class NameNullOrEmpty : DomainError
        {
            public NameNullOrEmpty() : base("ProjectTaskStatus.NameNullOrEmpty", "ProjectTask status is null or empty.")
            {
            }
        }
    }
}