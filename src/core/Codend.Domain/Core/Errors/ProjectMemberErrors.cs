namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// ProjectMember isFavourite domain errors.
    /// </summary>
    public static class ProjectMember
    {
        public class FavouriteDidntChange : DomainError
        {
            public FavouriteDidntChange()
                : base("ProjectMember.FavouriteDidntChange",
                    "Favourite did not change.")
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