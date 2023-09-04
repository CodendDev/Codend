using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

public class ProjectVersion : Entity<ProjectVersionId>, ISoftDeletableEntity
{
    private ProjectVersion(ProjectVersionId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }
    public ProjectId ProjectId { get; private set; }
    public ProjectVersionName Name { get; private set; }
    public ProjectVersionTag Tag { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public ProjectVersionChangelog Changelog { get; private set; }

    /// <summary>
    /// Creates new ProjectVersion instance.
    /// </summary>
    /// <param name="versionTag">Version tag.</param>
    /// <param name="projectId">Project Id it belongs to.</param>
    /// <param name="versionName">[Optional] Version name.</param>
    /// <param name="versionChangelog">[Optional] Version changelog.</param>
    /// <returns>Ok result with ProjectVersion object or an error.</returns>
    public static Result<ProjectVersion> Create(string versionTag, ProjectId projectId, string? versionName = null,
        string? versionChangelog = null)
    {
        var projectVersion = new ProjectVersion(new ProjectVersionId(Guid.NewGuid()))
        {
            ProjectId = projectId
        };

        var projectVersionTag = ProjectVersionTag.Create(versionTag);
        var projectVersionName = versionName is null ? null : ProjectVersionName.Create(versionName);
        var projectVersionChangelog =
            versionChangelog is null ? null : ProjectVersionChangelog.Create(versionChangelog);

        projectVersion.Tag = projectVersionTag.ValueOrDefault;
        if (projectVersionName is null)
        {
            projectVersion.Name = null;
            projectVersionName = Result.Ok();
        }
        else projectVersion.Name = projectVersionName.ValueOrDefault;

        if (projectVersionChangelog is null)
        {
            projectVersion.Changelog = null;
            projectVersionChangelog = Result.Ok();
        }
        else projectVersion.Changelog = projectVersionChangelog.ValueOrDefault;

        return Result.Ok(projectVersion)
            .MergeReasons(
                projectVersionTag.ToResult(),
                projectVersionName.ToResult(),
                projectVersionChangelog.ToResult());
    }

    /// <summary>
    /// Edits ProjectVersion instance.
    /// </summary>
    /// <param name="versionTag">New version tag.</param>
    /// <param name="versionName">New version name.</param>
    /// <param name="versionChangelog">New version changelog.</param>
    /// <returns>Ok result with ProjectVersion object or an error.</returns>
    public Result<ProjectVersion> Edit(string versionTag, string versionName,
        string versionChangelog)
    {
        var projectVersionTag = ProjectVersionTag.Create(versionTag);
        var projectVersionName = ProjectVersionName.Create(versionName);
        var projectVersionChangelog = ProjectVersionChangelog.Create(versionChangelog);

        Tag = projectVersionTag.ValueOrDefault;
        Name = projectVersionName.ValueOrDefault;
        Changelog = projectVersionChangelog.ValueOrDefault;

        return Result.Ok(this)
            .MergeReasons(
                projectVersionTag.ToResult(),
                projectVersionName.ToResult(),
                projectVersionChangelog.ToResult());
    }
}