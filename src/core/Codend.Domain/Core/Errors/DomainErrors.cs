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

    /// <summary>
    /// ProjectTask domain errors.
    /// </summary>
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

    /// <summary>
    /// Project domain errors.
    /// </summary>
    public static class ProjectName
    {
        public class NameTooLong : DomainError
        {
            public NameTooLong()
                : base("ProjectName.NameTooLong", "Given name is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("ProjectName.NullOrEmpty", "Given name is null or empty.")
            {
            }
        }
    }

    /// <summary>
    /// Project domain errors.
    /// </summary> 
    public static class ProjectDescription
    {
        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("ProjectDescription.NameTooLong", "Given description is too long.")
            {
            }
        }
    }
}