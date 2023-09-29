namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class ProjectTaskStatus
    {
        public class ProjectTaskStatusAlreadyExists : DomainError
        {
            public ProjectTaskStatusAlreadyExists()
                : base("ProjectTaskStatus.ProjectTaskStatusAlreadyExists",
                    "Task status with given properties already exists in this project.")
            {
            }
        }
        
        public class InvalidStatusId : DomainError
        {
            public InvalidStatusId()
                : base("ProjectTaskStatus.InvalidStatusId",
                    "ProjectTaskStatus is invalid for this project or status doesn't exist.")
            {
            }
        }
    }
}