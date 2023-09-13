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

    /// <summary>
    /// Sprint goal domain errors.
    /// </summary>
    public static class SprintGoal
    {
        public class GoalTooLong : DomainError
        {
            public GoalTooLong()
                : base("SprintGoal.GoalTooLong", "Given goal is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("SprintGoal.NullOrEmpty", "Given goal is null or empty.")
            {
            }
        }
    }
}