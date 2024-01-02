using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Events;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;

namespace Codend.Domain.Entities;

public class Project : DomainEventsAggregate<ProjectId>, ISoftDeletableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Project() : base(new ProjectId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    #region ISoftDeletableEntity properties

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    #endregion


    #region Project properties

    public const int MaxMembersCount = 25;
    public ProjectName Name { get; private set; }
    public ProjectDescription Description { get; private set; }
    public UserId OwnerId { get; private set; }
    public ProjectTaskStatusId DefaultStatusId { get; private set; }

    #endregion


    #region Domain methods

    public static Result<Project> Create(UserId ownerId, string name, string? description = null)
    {
        var project = new Project();

        var resultName = ProjectName.Create(name);
        var resultDescription = ProjectDescription.Create(description);

        var result = Result.Ok(project).MergeReasons(resultName.ToResult(), resultDescription.ToResult());

        if (result.IsFailed)
        {
            return result;
        }

        project.OwnerId = ownerId;
        project.Name = resultName.Value;
        project.Description = resultDescription.Value;

        return result;
    }

    /// <summary>
    /// Edits name of the Project, and validates new name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <returns>Ok result with ProjectName object or an error.</returns>
    public Result<ProjectName> EditName(string name)
    {
        var result = ProjectName.Create(name);
        if (result.IsFailed)
        {
            return result;
        }

        Name = result.Value;

        return result;
    }

    /// <summary>
    /// Edits description of the Project, and validates new description.
    /// </summary>
    /// <param name="description">New description.</param>
    /// <returns>Ok result with ProjectDescription object or an error.</returns>
    public Result<ProjectDescription> EditDescription(string? description)
    {
        var result = ProjectDescription.Create(description);
        if (result.IsFailed)
        {
            return result;
        }

        Description = result.Value;

        return result;
    }

    /// <summary>
    /// Releases new Project version.
    /// </summary>
    /// <param name="versionTag">Version tag.</param>
    /// <param name="versionName">[Optional] Version name.</param>
    /// <param name="versionChangelog">[Optional] Version changelog.</param>
    /// <returns>Ok result with ProjectVersion object or an error.</returns>
    public Result<ProjectVersion> ReleaseVersion(string versionTag, string? versionName = null,
        string? versionChangelog = null)
    {
        var result = ProjectVersion.Create(versionTag, Id, versionName, versionChangelog);
        if (result.IsFailed)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// Edits Project version.
    /// </summary>
    /// <param name="projectVersion">Project version to be edited.</param>
    /// <param name="versionTag">New version tag.</param>
    /// <param name="versionName">New version name.</param>
    /// <param name="versionChangelog">New version changelog.</param>
    /// <returns>Ok result with ProjectVersion object or an error.</returns>
    public Result<ProjectVersion> EditVersion(ProjectVersion projectVersion, string versionTag,
        string versionName, string versionChangelog)
    {
        var result = projectVersion.Edit(versionTag, versionName, versionChangelog);
        if (result.IsFailed)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// Creates new sprint.
    /// </summary>
    /// <param name="name">Sprint name.</param>
    /// <param name="startDate">Sprint startDate.</param>
    /// <param name="endDate">Sprint endDate.</param>
    /// <param name="goal">Sprint goal.</param>
    /// <returns>Ok result with Sprint object or an error.</returns>
    public Result<Sprint> CreateSprint(string name, DateTime startDate, DateTime endDate, string? goal)
    {
        var result = Sprint.Create(name, Id, startDate, endDate, goal);
        if (result.IsFailed)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// Adds user to project.
    /// </summary>
    /// <param name="userId">User to be added.</param>
    public Result AddUserToProject(ProjectMember userId)
    {
        var evt = new UserAddedToProjectEvent(Id, userId);
        Raise(evt);
        return Result.Ok();
    }

    /// <summary>
    /// Removes user from project.
    /// </summary>
    /// <param name="userId">User to be removed.</param>
    public Result RemoveUserFromProject(UserId userId) => Result.Ok();

    /// <summary>
    /// Creates and adds projectTask status to project
    /// </summary>
    /// <param name="statusName">Name for the new status.</param>
    /// <param name="position">Position for the new status.</param>
    public Result<ProjectTaskStatus> AddProjectTaskStatusToProject(string statusName, Lexorank? position)
    {
        var result = ProjectTaskStatus.Create(Id, statusName, position);

        if (result.IsFailed)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// Edits projectTask status.
    /// </summary>
    /// <param name="status">Status to be edited.</param>
    /// <param name="statusName">New status name.</param>
    public Result<ProjectTaskStatus> EditProjectTaskStatus(ProjectTaskStatus status, string statusName)
    {
        var result = status.EditName(statusName);

        if (result.IsFailed)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// Changes project default status id to one of Project defined or default statuses.
    /// </summary>
    /// <param name="defaultStatusId">New default status id.</param>
    /// <returns>Ok result with ProjectTaskStatusId object.</returns>
    public Result<ProjectTaskStatusId> EditDefaultStatus(ProjectTaskStatusId defaultStatusId)
    {
        DefaultStatusId = defaultStatusId;

        return Result.Ok(defaultStatusId);
    }

    #endregion
}