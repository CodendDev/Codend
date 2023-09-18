namespace Codend.Domain.Core.Errors;

public static partial class DomainErrors
{
    /// <summary>
    /// ProjectMember isFavourite domain errors.
    /// </summary>
    public static class ProjectMember
    {
        public class IsMemberFavouriteAlready : DomainError
        {
            public IsMemberFavouriteAlready()
                : base("ProjectMember.IsMemberFavouriteAlready",
                    "Project is already member favourite project.")
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