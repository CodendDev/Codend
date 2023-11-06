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

    #region Sprint properties

    public SprintName Name { get; private set; }
    public SprintPeriod Period { get; private set; }
    public SprintGoal Goal { get; private set; }
    public ProjectId ProjectId { get; private set; }

    #endregion

    #region ISoftDeletableEntity properties

    public DateTime DeletedOnUtc { get; private set; }
    public bool Deleted { get; }

    #endregion

    #region Domain methods

    /// <summary>
    /// Creates new Sprint.
    /// </summary>
    /// <param name="name">Sprint name.</param>
    /// <param name="projectId">Project Id it belongs to.</param>
    /// <param name="startDate">Sprint startDate.</param>
    /// <param name="endDate">Sprint endDate.</param>
    /// <param name="goal">[Optional] Sprint goal.</param>
    /// <returns>Ok result with Sprint object or an error.</returns>
    public static Result<Sprint> Create(
        string name,
        ProjectId projectId,
        DateTime startDate,
        DateTime endDate,
        string? goal)
    {
        var sprint = new Sprint()
        {
            ProjectId = projectId
        };

        var nameResult = SprintName.Create(name);
        var periodResult = SprintPeriod.Create(startDate, endDate);
        var goalResult = SprintGoal.Create(goal);

        sprint.Name = nameResult.ValueOrDefault;
        sprint.Period = periodResult.ValueOrDefault;
        sprint.Goal = goalResult.ValueOrDefault;

        return Result.Ok(sprint)
            .MergeReasons(
                nameResult.ToResult(),
                periodResult.ToResult(),
                goalResult.ToResult()
            );
    }

    /// <summary>
    /// Edits sprint name.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <returns>Ok <see cref="Result"/> with new name or failure with errors.</returns>
    public Result<SprintName> EditName(string name)
    {
        var resultName = SprintName.Create(name);
        if (resultName.IsFailed)
        {
            return resultName;
        }

        Name = resultName.Value;
        return resultName;
    }

    /// <summary>
    /// Edits sprint goal.
    /// </summary>
    /// <param name="goal">New goal.</param>
    /// <returns>Ok <see cref="Result"/> with new goal or failure with errors.</returns>
    public Result<SprintGoal> EditGoal(string? goal)
    {
        var sprintGoal = SprintGoal.Create(goal);
        if (sprintGoal.IsFailed)
        {
            return sprintGoal;
        }

        Goal = sprintGoal.Value;
        return sprintGoal;
    }

    /// <summary>
    /// Edits sprint period.
    /// </summary>
    /// <param name="startDate">Sprint start date.</param>
    /// <param name="endDate">Sprint end date.</param>
    /// <returns>Ok <see cref="Result"/> with new period or failure with errors.</returns>
    public Result<SprintPeriod> EditPeriod(DateTime startDate, DateTime endDate)
    {
        var sprintPeriod = SprintPeriod.Create(startDate, endDate);
        if (sprintPeriod.IsFailed)
        {
            return sprintPeriod;
        }

        Period = sprintPeriod.Value;
        return sprintPeriod;
    }

    public Result<IEnumerable<SprintProjectTask>> AssignTasks(IEnumerable<ISprintTaskId> taskIds)
    {
        var results = taskIds
            .Select(taskId => taskId switch
            {
                ProjectTaskId projectTaskId => SprintProjectTask.Create(Id, projectTaskId),
                StoryId storyId => SprintProjectTask.Create(Id, storyId),
                EpicId epicId => SprintProjectTask.Create(Id, epicId),
                _ => throw new ArgumentOutOfRangeException(nameof(taskId), taskId, null)
            })
            .Select(r => r.ValueOrDefault);

        return Result.Ok(results);
    }

    #endregion
}