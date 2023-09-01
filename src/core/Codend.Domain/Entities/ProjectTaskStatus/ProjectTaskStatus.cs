using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

public class ProjectTaskStatus : Entity<ProjectTaskStatusId>
{
    private ProjectTaskStatus() : base(new ProjectTaskStatusId(Guid.NewGuid()))
    {
    }

    public ProjectTaskStatusName Name { get; private set; }
    public ProjectId ProjectId { get; private set; }

    /// <summary>
    /// Creates new projectTaskStatus.
    /// </summary>
    /// <param name="projectId">Id of project it belongs to.</param>
    /// <param name="name">Name for the status.</param>
    /// <returns>Ok result with newly created ProjectTaskStatus object or an error.</returns>
    public static Result<ProjectTaskStatus> Create(ProjectId projectId, string name)
    {
        var status = new ProjectTaskStatus()
        {
            ProjectId = projectId
        };

        var nameResult = ProjectTaskStatusName.Create(name);
        status.Name = nameResult.ValueOrDefault;

        return Result.Ok(status).MergeReasons(nameResult.ToResult());
    }

    /// <summary>
    /// Edits name of the ProjectTaskStatus.
    /// </summary>
    /// <param name="name">Name for the status.</param>
    /// <returns>Ok result with ProjectTaskStatus object witch new name or an error.</returns>
    public Result<ProjectTaskStatus> EditName(string name)
    {
        var nameResult = ProjectTaskStatusName.Create(name);
        this.Name = nameResult.ValueOrDefault;

        return Result.Ok(this).MergeReasons(nameResult.ToResult());
    }
}