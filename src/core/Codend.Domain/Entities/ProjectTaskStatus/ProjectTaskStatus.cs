using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using Codend.Shared.Infrastructure.Lexorank;
using FluentResults;

namespace Codend.Domain.Entities;

public class ProjectTaskStatus : Entity<ProjectTaskStatusId>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ProjectTaskStatus() : base(new ProjectTaskStatusId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public ProjectTaskStatusName Name { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public Lexorank? Position { get; private set; }

    /// <summary>
    /// Creates new projectTaskStatus.
    /// </summary>
    /// <param name="projectId">Id of project it belongs to.</param>
    /// <param name="name">Name for the status.</param>
    /// <param name="position">Status position.</param>
    /// <returns>Ok result with newly created ProjectTaskStatus object or an error.</returns>
    public static Result<ProjectTaskStatus> Create(ProjectId projectId, string name, Lexorank? position)
    {
        var status = new ProjectTaskStatus()
        {
            ProjectId = projectId
        };

        var nameResult = ProjectTaskStatusName.Create(name);
        status.Name = nameResult.ValueOrDefault;
        status.Position = position;

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

    /// <summary>
    /// Edits status position.
    /// </summary>
    /// <param name="position">Lexorank position.</param>
    /// <returns>Ok result with edited <see cref="ProjectTaskStatus"/>.</returns>
    public Result<ProjectTaskStatus> EditPosition(Lexorank? position)
    {
        Position = position;
        return Result.Ok(this);
    }
}