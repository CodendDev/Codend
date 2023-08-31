using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Events;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

public class Project : Aggregate<ProjectId>, ISoftDeletableEntity
{
    public Project() : base(new ProjectId(Guid.NewGuid()))
    {
        ProjectTasks = new List<ProjectTask>();
        ProjectVersions = new List<ProjectVersion>();
        Sprints = new List<Sprint>();
        ProjectMembers = new List<User>();
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public virtual List<ProjectTask> ProjectTasks { get; private set; }

    public virtual List<ProjectVersion> ProjectVersions { get; private set; }

    public virtual List<Sprint> Sprints { get; private set; }

    public ProjectName ProjectName { get; private set; }

    public ProjectDescription? ProjectDescription { get; private set; }

    public UserId OwnerId { get; private set; }

    public virtual List<User> ProjectMembers { get; private set; }

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

        ProjectName = result.Value;

        var evt = new ProjectNameEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Edits Project description, and validates new desc.
    /// </summary>
    /// <param name="description">New description.</param>
    /// <returns>Ok result with ProjectDescription object or an error.</returns>
    public Result<ProjectDescription> EditDescription(string description)
    {
        var result = ProjectDescription.Create(description);
        if (result.IsFailed)
        {
            return result;
        }

        ProjectDescription = result.Value;

        var evt = new ProjectDescriptionEditedEvent(result.Value, Id);
        Raise(evt);

        return result;
    }

    /// <summary>
    /// Adds new task to project.
    /// </summary>
    /// <param name="task">Task to be added.</param>
    public void AddTask(ProjectTask task)
    {
        var evt = new ProjectTaskAddedEvent(task, Id);
        Raise(evt);
    }

    /// <summary>
    /// Deletes task from project.
    /// </summary>
    /// <param name="task">Task to be deleted.</param>
    public void DeleteTask(ProjectTask task)
    {
        var evt = new ProjectTaskDeletedEvent(task, Id);
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
    public void AddTaskToSprint(Sprint sprint, ProjectTask task)
    {
        var evt = new ProjectTaskAddedToSprintEvent(sprint, task);
        Raise(evt);
    }

    /// <summary>
    /// Removes task from sprint.
    /// </summary>
    /// <param name="task">Task to be removed from sprint.</param>
    /// <param name="sprint">Sprint from which we remove a task.</param>
    public void RemoveTaskFromSprint(Sprint sprint, ProjectTask task)
    {
        var evt = new ProjectTaskRemovedFromSprintEvent(sprint, task);
        Raise(evt);
    }
}