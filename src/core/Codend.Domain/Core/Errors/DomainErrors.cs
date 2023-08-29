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
                : base("ProjectDescription.DescriptionTooLong", "Given description is too long.")
            {
            }
        }
    }


    /// <summary>
    /// Project version changelog domain errors.
    /// </summary>
    public static class ProjectVersionChangelog
    {
        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("ProjectVersionChangelog.DescriptionTooLong", "Given changelog is too long.")
            {
            }
        }
    }

    /// <summary>
    /// Project version name domain errors.
    /// </summary>
    public static class ProjectVersionName
    {
        public class NameTooLong : DomainError
        {
            public NameTooLong()
                : base("ProjectVersionName.NameTooLong", "Given name is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("ProjectVersionName.NullOrEmpty", "Given name is null or empty.")
            {
            }
        }
    }
    
    /// <summary>
    /// Project version tag domain errors.
    /// </summary>
    public static class ProjectVersionTag
    {
        public class TagTooLong : DomainError
        {
            public TagTooLong()
                : base("ProjectVersionTag.TagTooLong", "Given tag is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("ProjectVersionTag.NullOrEmpty", "Given tag is null or empty.")
            {
            }
        }
    }
}