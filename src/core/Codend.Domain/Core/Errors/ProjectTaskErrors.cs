namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
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

    public static class ProjectTaskPriority
    {
        public class InvalidPriorityName : DomainError
        {
            public InvalidPriorityName()
                : base(
                    "ProjectTaskPriority.InvalidPriorityName",
                    $"Only ['{string.Join("', '", Enums.ProjectTaskPriority.DefaultList())}'] are allowed")
            {
            }
        }
    }

    public static class ProjectTaskErrors
    {
        public class ProjectTaskNotFound : DomainError
        {
            public ProjectTaskNotFound()
                : base("ProjectTaskErrors.ProjectTaskNotFound", "ProjectTask not found in database.")
            {
            }
        }

        public class InvalidStatusId : DomainError
        {
            public InvalidStatusId()
                : base("ProjectTaskErrors.InvalidStatusId",
                    "ProjectTaskStatus is invalid for this project or project doesn't exist.")
            {
            }
        }
    }
}