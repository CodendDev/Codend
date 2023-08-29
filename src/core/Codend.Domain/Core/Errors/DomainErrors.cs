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
                : base("ProjectDescription.NameTooLong", "Given description is too long.")
            {
            }
        }
    }


    public static class ProjectVersionChangelog
    {
        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("ProjectVersionChangelog.NameTooLong", "Given changelog is too long.")
            {
            }
        }
    }
}