using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Create;
using Codend.Domain.Core.Primitives;
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
                EstimatedTime.ToTimeSpan(),
                DueDate,
                StoryPoints,
                EntityIdExtensions.ToKeyGuid<UserId>(AssigneeId),
                EntityIdExtensions.ToKeyGuid<StoryId>(StoryId)
            ));

        return command;
    }
}