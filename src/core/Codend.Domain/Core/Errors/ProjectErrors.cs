namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// General Project errors.
    /// </summary>
    public static class ProjectErrors
    {
        public class ProjectNotFound : DomainError
        {
            public ProjectNotFound() : base("ProjectErrors.ProjectNotFound", "Project not found in database.")
            {
            }
        }
    }
    
    /// <summary>
    /// Project name domain errors.
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
    /// Project description domain errors.
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
}