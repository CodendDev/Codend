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
    }
}