namespace Codend.Domain.Core.Abstractions;

public interface ISprintTask
{
    Guid SprintTaskId { get; }

    /// <summary>
    /// String representation of task type. 
    /// </summary>
    string SprintTaskType { get; }

    string SprintTaskName { get; }

    Guid SprintTaskStatusId { get; }

    string? SprintTaskPriority { get; }

    Guid? SprintTaskRelatedTaskId { get; }
}