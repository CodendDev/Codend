namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class ProjectTaskPriority
    {
        public class InvalidPriorityName : DomainError
        {
            public InvalidPriorityName()
                : base("ProjectTaskPriority.InvalidPriorityName",
                    $"Only ['{string.Join("', '", Enums.ProjectTaskPriority.DefaultList())}'] are allowed")
            {
            }
        }
    }

    public static class ProjectTaskErrors
    {
        public class InvalidAssigneeId : DomainError
        {
            public InvalidAssigneeId()
                : base("ProjectTaskErrors.InvalidAssigneeId",
                    "User with given does not exist or is not a member of the project.")
            {
            }
        }

        public class InvalidStoryId : DomainError
        {
            public InvalidStoryId()
                : base("ProjectTaskErrors.InvalidStoryId",
                    "Story with given does not exist or is not a member of the project.")
            {
            }
        }
    }
}