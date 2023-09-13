namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    public static class StoryDescription
    {
        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("StoryDescription.NullOrEmpty", "Story description can't be null or empty.")
            {
            }
        }

        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("StoryDescription.DescriptionTooLong", "Story description is too long.")
            {
            }
        }
    }

    public static class StoryName
    {
        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("StoryName.NullOrEmpty", "Story name can't be null or empty.")
            {
            }
        }

        public class NameTooLong : DomainError
        {
            public NameTooLong()
                : base("StoryName.DescriptionTooLong", "Story name is too long.")
            {
            }
        }
    }

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