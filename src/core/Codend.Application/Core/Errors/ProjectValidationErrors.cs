namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
public static partial class ValidationErrors
{
    public static class Project
    {
        public class NotFoundOrUserUnauthorized : ValidationError
        {
            public NotFoundOrUserUnauthorized() : base("Project.NotFoundOrUserUnauthorized",
                $"Project with given id not found or user is unauthorized.")
            {
            }
        }
    }
}