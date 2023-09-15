using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Create;
using Codend.Domain.Core.Primitives;
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
                EstimatedTime.ToTimeSpan(),
                DueDate,
                StoryPoints,
                EntityIdExtensions.ToKeyGuid<UserId>(AssigneeId),
                EntityIdExtensions.ToKeyGuid<StoryId>(StoryId)
            ));

        return command;
    }
}