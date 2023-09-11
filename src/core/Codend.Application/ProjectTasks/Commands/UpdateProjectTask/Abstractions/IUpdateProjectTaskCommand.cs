using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

public interface IUpdateProjectTaskCommand
{
    /// <summary>
    /// Id of <see cref="AbstractProjectTask"/> which will be updated.
    /// </summary>
    ProjectTaskId TaskId { get; }

    IShouldUpdate<string> Name { get; }
    IShouldUpdate<string> Priority { get; }
    IShouldUpdate<ProjectTaskStatusId> StatusId { get; }
    IShouldUpdate<string?> Description { get; }
    IShouldUpdate<TimeSpan?> EstimatedTime { get; }
    IShouldUpdate<DateTime?> DueDate { get; }
    IShouldUpdate<uint?> StoryPoints { get; }
    IShouldUpdate<UserId?> AssigneeId { get; }
}