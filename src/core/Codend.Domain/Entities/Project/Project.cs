using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Events;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

public class Project : Aggregate<ProjectId>, ISoftDeletableEntity
{
    private Project() : base(new ProjectId(Guid.NewGuid()))
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }
    public ProjectName Name { get; private set; }
    public ProjectDescription Description { get; private set; }
    public UserId OwnerId { get; private set; }

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
    /// Edits name and description of the Project, and validates new name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <param name="description">New description</param>
    /// <returns>Ok result with ProjectName object or an error.</returns>
    public Result<Project> Edit(string name, string? description)
    {
        var resultName = ProjectName.Create(name);
        var resultDescription = ProjectDescription.Create(description);

        var result = Result.Ok(this).MergeReasons(resultName.ToResult(), resultDescription.ToResult());

        if (result.IsFailed)
        {
            return result;
        }

        Name = resultName.Value;
        Description = resultDescription.Value;

        var evt = new ProjectEditedEvent(this);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Adds new task to project.
    /// </summary>
    /// <param name="task">Task to be added.</param>
    public void AddTask(AbstractProjectTask task)
    {
        var evt = new ProjectTaskAddedToProjectEvent(task, Id);
        Raise(evt);
    }

    /// <summary>
    /// Deletes task from project.
    /// </summary>
    /// <param name="task">Task to be deleted.</param>
    public void DeleteTask(AbstractProjectTask task)
    {
        var evt = new ProjectTaskDeletedFromProjectEvent(task, Id);
        Raise(evt);
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

        var evt = new ProjectVersionReleasedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Deletes Project version.
    /// </summary>
    /// <param name="projectVersion">ProjectVersion to be deleted.</param>
    public void DeleteVersion(ProjectVersion projectVersion)
    {
        var evt = new ProjectVersionDeletedEvent(projectVersion, Id);
        Raise(evt);
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

        var evt = new ProjectVersionEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Creates new sprint.
    /// </summary>
    /// <param name="startDate">Sprint startDane.</param>
    /// <param name="endDate">Sprint endDate.</param>
    /// <param name="goal">Sprint goal.</param>
    /// <returns>Ok result with Sprint object or an error.</returns>
    public Result<Sprint> CreateSprint(DateTime startDate, DateTime endDate, string? goal)
    {
        var result = Sprint.Create(Id, startDate, endDate, goal);
        if (result.IsFailed)
        {
            return result;
        }

        var evt = new SprintCreatedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Adds task to sprint.
    /// </summary>
    /// <param name="task">Task to be added to sprint.</param>
    /// <param name="sprint">Sprint to which we add a task.</param>
    public void AddTaskToSprint(Sprint sprint, AbstractProjectTask task)
    {
        var evt = new ProjectTaskAddedToSprintEvent(sprint, task);
        Raise(evt);
    }

    /// <summary>
    /// Removes task from sprint.
    /// </summary>
    /// <param name="task">Task to be removed from sprint.</param>
    /// <param name="sprint">Sprint from which we remove a task.</param>
    public void RemoveTaskFromSprint(Sprint sprint, AbstractProjectTask task)
    {
        var evt = new ProjectTaskRemovedFromSprintEvent(sprint, task);
        Raise(evt);
    }

    /// <summary>
    /// Adds user to project.
    /// </summary>
    /// <param name="userId">User to be added.</param>
    public void AddUserToProject(UserId userId)
    {
        var evt = new UserAddedToProjectEvent(userId, Id);
        Raise(evt);
    }

    /// <summary>
    /// Removes user from project.
    /// </summary>
    /// <param name="userId">User to be removed.</param>
    public void RemoveUserFromProject(UserId userId)
    {
        var evt = new UserRemovedFromProjectEvent(userId, Id);
        Raise(evt);
    }

    /// <summary>
    /// Creates and adds projectTask status to project
    /// </summary>
    /// <param name="statusName">Name for the new status.</param>
    public Result<ProjectTaskStatus> AddProjectTaskStatusToProject(string statusName)
    {
        var result = ProjectTaskStatus.Create(Id, statusName);

        if (result.IsFailed)
        {
            return result;
        }

        var evt = new ProjectTaskStatusAddedToProjectEvent(result.Value, Id);
        Raise(evt);

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

        var evt = new ProjectTaskStatusEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Removes ProjectTaskStatus from project.
    /// </summary>
    /// <param name="status">Status to be removed.</param>
    public void RemoveProjectTaskStatusFromProject(ProjectTaskStatus status)
    {
        var evt = new ProjectTaskStatusRemovedFromProjectEvent(status, Id);
        Raise(evt);
    }
}