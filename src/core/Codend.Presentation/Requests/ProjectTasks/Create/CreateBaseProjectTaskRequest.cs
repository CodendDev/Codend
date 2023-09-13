using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Create;
using Codend.Domain.Entities;
using Codend.Presentation.Requests.Abstractions;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

/// <inheritdoc cref="Codend.Contracts.Requests.ProjectTasks.Create.ICreateBaseProjectTaskRequest" />
public sealed record CreateBaseProjectTaskRequest
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
) : ICreateBaseProjectTaskRequest, IMapRequestToCommand<CreateBaseProjectTaskCommand, Guid>
{
    /// <inheritdoc />
    public CreateBaseProjectTaskCommand MapToCommand()
    {
        var command = new CreateBaseProjectTaskCommand(
            new BaseProjectTaskCreateProperties(
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