using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Create;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Presentation.Requests.Abstractions;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

/// <inheritdoc cref="Codend.Contracts.Requests.ProjectTasks.Create.ICreateBugfixProjectTaskRequest" />
public sealed record CreateBugfixProjectTaskRequest
(
    string Name,
    string Priority,
    Guid StatusId,
    Guid ProjectId,
    string? Description,
    EstimatedTimeRequest? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    Guid? StoryId
) : ICreateBugfixProjectTaskRequest, IMapRequestToCommand<CreateBugfixProjectTaskCommand, Guid>
{
    /// <inheritdoc />
    public CreateBugfixProjectTaskCommand MapToCommand()
    {
        var command = new CreateBugfixProjectTaskCommand(
            new BugfixProjectTaskCreateProperties(
                Name,
                Priority,
                new ProjectTaskStatusId(StatusId),
                new ProjectId(ProjectId),
                Description,
                EstimatedTime is not null
                    ? new TimeSpan(
                        (int)EstimatedTime.Days,
                        (int)EstimatedTime.Hours,
                        (int)EstimatedTime.Minutes, 0)
                    : null,
                DueDate,
                StoryPoints,
                AssigneeId is not null ? new UserId(AssigneeId.Value) : null,
                StoryId is not null ? new StoryId(StoryId.Value) : null
            ));

        return command;
    }
}