namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// Project version changelog domain errors.
    /// </summary>
    public static class ProjectVersionChangelog
    {
        public class DescriptionTooLong : DomainError
        {
            public DescriptionTooLong()
                : base("ProjectVersionChangelog.DescriptionTooLong", "Given changelog is too long.")
            {
            }
        }
    }

    /// <summary>
    /// Project version name domain errors.
    /// </summary>
    public static class ProjectVersionName
    {
        public class NameTooLong : DomainError
        {
            public NameTooLong()
                : base("ProjectVersionName.NameTooLong", "Given name is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("ProjectVersionName.NullOrEmpty", "Given name is null or empty.")
            {
            }
        }
    }

    /// <summary>
    /// Project version tag domain errors.
    /// </summary>
    public static class ProjectVersionTag
    {
        public class TagTooLong : DomainError
        {
            public TagTooLong()
                : base("ProjectVersionTag.TagTooLong", "Given tag is too long.")
            {
            }
        }

        public class NullOrEmpty : DomainError
        {
            public NullOrEmpty()
                : base("ProjectVersionTag.NullOrEmpty", "Given tag is null or empty.")
            {
            }
        }
    }
}