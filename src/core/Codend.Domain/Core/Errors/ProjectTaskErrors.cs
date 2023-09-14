namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
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