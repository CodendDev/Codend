namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// ProjectMember isFavourite domain errors.
    /// </summary>
    public static class ProjectMember
    {
        public class IsFavouriteNotChanged : DomainError
        {
            public IsFavouriteNotChanged()
                : base("ProjectMember.IsFavouriteNotChanged",
                    "New IsFavourite value is the same as current IsFavourite.")
            {
            }
        }

        public class UserIsProjectMemberAlready : DomainError
        {
            public UserIsProjectMemberAlready()
                : base("ProjectMember.UserIsMemberAlready", "User is member of project already.")
            {
            }
        }
    }
}