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
}