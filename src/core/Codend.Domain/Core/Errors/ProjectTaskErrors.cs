namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class ProjectTaskErrors
    {
        public class ProjectTaskNotFound : DomainErrors.DomainError
        {
            public ProjectTaskNotFound()
                : base("ProjectTaskErrors.ProjectTaskNotFound", "ProjectTask not found in database.")
            {
            }
        }
    }
}