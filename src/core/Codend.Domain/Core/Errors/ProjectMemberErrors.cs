namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// ProjectMember isFavourite domain errors.
    /// </summary>
    public static class ProjectMemberIsFavourite
    {
        public class IsFavouriteNotChanged : DomainError
        {
            public IsFavouriteNotChanged()
                : base("ProjectMember.IsFavouriteNotChanged",
                    "New IsFavourite value is the same as current IsFavourite.")
            {
            }
        }
    }
}