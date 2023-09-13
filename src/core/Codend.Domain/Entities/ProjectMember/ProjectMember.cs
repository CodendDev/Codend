using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using FluentResults;

namespace Codend.Domain.Entities;

/// <summary>
/// Project member entity, which describes user's affiliation with the project.
/// </summary>
public class ProjectMember : Entity<ProjectMemberId>
{
    private ProjectMember() : base(new ProjectMemberId(Guid.NewGuid()))
    {
    }

    public ProjectId ProjectId { get; private set; }
    public UserId MemberId { get; private set; }
    public bool IsFavourite { get; private set; }
    

    public static Result<ProjectMember> Create(ProjectId projectId, UserId memberId)
    {
        return new ProjectMember()
        {
            ProjectId = projectId,
            MemberId = memberId,
            IsFavourite = false
        };
    }
}