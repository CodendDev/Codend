using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;
using FluentResults;

namespace Codend.Domain.Entities;

public class Sprint : Entity<SprintId>, ISoftDeletableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Sprint() : base(new SprintId(Guid.NewGuid()))
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public SprintPeriod Period { get; private set; }
    public SprintGoal Goal { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public DateTime DeletedOnUtc { get; private set; }
    public bool Deleted { get; }

    /// <summary>
    /// Creates new Sprint.
    /// </summary>
    /// <param name="projectId">Project Id it belongs to.</param>
    /// <param name="startDate">Sprint startDate.</param>
    /// <param name="endDate">Sprint endDate.</param>
    /// <param name="goal">[Optional] Sprint goal.</param>
    /// <returns>Ok result with Sprint object or an error.</returns>
    public static Result<Sprint> Create(ProjectId projectId, DateTime startDate, DateTime endDate, string? goal)
    {
        var sprint = new Sprint()
        {
            ProjectId = projectId
        };

        var periodResult = SprintPeriod.Create(startDate, endDate);
        var goalResult = SprintGoal.Create(goal);

        sprint.Period = periodResult.ValueOrDefault;
        sprint.Goal = goalResult.ValueOrDefault;

        return Result.Ok(sprint)
            .MergeReasons(
                periodResult.ToResult(),
                goalResult.ToResult());
    }
}