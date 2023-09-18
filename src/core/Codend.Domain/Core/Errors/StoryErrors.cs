namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class StoryErrors
    {
        public class InvalidEpicId : DomainError
        {
            public InvalidEpicId()
                : base("InvalidEpicId.InvalidEpicId", "EpicId is invalid for this story or epic doesn't exist.")
            {
            }
        }
    }
}