namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class StoryErrors
    {
        public class StoryNotFound : DomainError
        {
            public StoryNotFound() : base("StoryErrors.StoryNotFound", "Story not found in database.")
            {
            }
        }
    }
}