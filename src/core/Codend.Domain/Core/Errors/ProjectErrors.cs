namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class ProjectErrors
    {
        public class ProjectNotFound : DomainError
        {
            public ProjectNotFound() : base("ProjectErrors.ProjectNotFound", "Project not found in database.")
            {
            }
        }
    }
}