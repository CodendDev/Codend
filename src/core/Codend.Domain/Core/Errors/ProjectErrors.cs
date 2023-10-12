namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class Project
    {
        public class ProjectHasToHaveProjectTaskStatus : DomainError
        {
            public ProjectHasToHaveProjectTaskStatus()
                : base("Project.ProjectHasToHaveProjectTaskStatus", "Project has to have at least one task status")
            {
            }
        }

        public class ProjectHasMaximumNumberOfMembers : DomainError
        {
            public ProjectHasMaximumNumberOfMembers()
                : base("Project.ProjectHasMaximumNumberOfMembers",
                    $"Project already has the maximum number of members equal {Entities.Project.MaxMembersCount}.")
            {
            }
        }
        
        public class InvalidDefaultStatusId : DomainError
        {
            public InvalidDefaultStatusId()
                : base("Project.InvalidDefaultStatusId",
                    "Status id is invalid for this project or status does not exist.")
            {
            }
        }
    }
}