using Codend.Domain.Core.Enums;

namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
public static partial class ValidationErrors
{
    /// <summary>
    /// Project task validation error static class.
    /// </summary>
    public static class ProjectTask
    {
        /// <inheritdoc />
        public class NotFoundOrUserUnauthorized : ValidationError
        {
            /// <inheritdoc />
            public NotFoundOrUserUnauthorized() : base("ProjectTask.NotFoundOrUserUnauthorized",
                $"ProjectTask with given id not found or user is unauthorized.")
            {
            }
        }

        /// <inheritdoc />
        public class PriorityNotDefined : ValidationError
        {
            /// <inheritdoc />
            public PriorityNotDefined() : base("ProjectTask.PriorityNotDefined",
                $"Given priority is not defined. Valid priorities: '{string.Join(',',ProjectTaskPriority.DefaultList())}'")
            {
            }
        }
        
        /// <inheritdoc />
        public class AssigneeNotExistOrIsNotProjectMember : ValidationError
        {
            /// <inheritdoc />
            public AssigneeNotExistOrIsNotProjectMember() : base("ProjectTask.AssigneeNotExistOrIsNotProjectMember",
                "Assignee does not exist or is not a project member.")
            {
            }
        }
        
        /// <inheritdoc />
        public class StoryNotExist : ValidationError
        {
            /// <inheritdoc />
            public StoryNotExist() : base("ProjectTask.AssigneeNotExistOrIsNotProjectMember",
                "Assignee does not exist or is not a project member.")
            {
            }
        }
    }
}