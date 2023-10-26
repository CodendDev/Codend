using Codend.Domain.Entities;

namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// Sprint period domain errors.
    /// </summary>
    public static class SprintPeriod
    {
        public class StartDateAfterEndDate : DomainError
        {
            public StartDateAfterEndDate()
                : base("SprintPeriod.StartDateAfterEndDate", "Start date must be before end date.")
            {
            }
        }
    }

    public static class Sprint
    {
        public class TaskDoesntExistInProject : DomainError
        {
            public TaskDoesntExistInProject()
                : base("Sprint.TaskDoesntExistInProject", $"Task doesn't exist in project.")
            {
            }
        }
    }
}