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

    /// <summary>
    /// Sprint period domain errors.
    /// </summary>
    public static class SprintPeriod
    {
        public class StartDateAfterEndDate : DomainError
        {
            public StartDateAfterEndDate()
                : base("SprintPeriod.StartDateAfterEndDate", "Start date must be before end date.")
            {
            }
        }
    }

    /// <summary>
    /// Sprint goal domain errors.
    /// </summary>
    public static class SprintGoal
    {
        public class GoalTooLong : DomainError
        {
            public GoalTooLong()
                : base("SprintGoal.GoalTooLong", "Given goal is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("SprintGoal.NullOrEmpty", "Given goal is null or empty.")
            {
            }
        }
    }
}