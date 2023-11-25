using Codend.Domain.Core.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectMember;

namespace Codend.Domain.Entities;

/// <summary>
/// Project member entity, which describes user's affiliation with the project.
/// </summary>
public class ProjectMember : Entity<ProjectMemberId>, IUser
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ProjectMember() : base(new ProjectMemberId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public ProjectId ProjectId { get; private set; }
    public UserId MemberId { get; private set; }
    public bool IsFavourite { get; private set; }
    public bool NotificationEnabled { get; private set; }

    public static Result<ProjectMember> Create(ProjectId projectId, UserId memberId)
    {
        var projectMember = new ProjectMember()
        {
            ProjectId = projectId,
            MemberId = memberId,
            IsFavourite = false,
            NotificationEnabled = true
        };

        return Result.Ok(projectMember);
    }

    public Result<ProjectMember> SetIsFavourite(bool isFavourite)
    {
        if (IsFavourite == isFavourite)
        {
            return Result.Fail(new FavouriteDidntChange());
        }

        IsFavourite = isFavourite;

        return Result.Ok(this);
    }

    public Result<ProjectMember> EnableNotifications()
    {
        NotificationEnabled = true;
        return Result.Ok(this);
    }

    public Result<ProjectMember> DisableNotifications()
    {
        NotificationEnabled = false;
        return Result.Ok(this);
    }

    public Guid UserId => MemberId.Value;
}