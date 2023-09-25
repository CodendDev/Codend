namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public class Project
    {
        public class ProjectHasToHaveProjectTaskStatus : DomainError
        {
            public ProjectHasToHaveProjectTaskStatus()
                : base("Project.ProjectHasToHaveProjectTaskStatus", "Project has to have at least one task status")
            {
            }
        }
    }
}