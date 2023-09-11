using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.Requests;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

public record CreateBugfixProjectTaskRequest
(
    string Name,
    string Priority,
    Guid StatusId,
    Guid ProjectId,
    string? Description,
    EstimatedTimeRequest? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId
) : AbstractCreateProjectTaskRequest<CreateBugfixProjectTaskCommand>
(
    Name,
    Priority,
    StatusId,
    ProjectId,
    Description,
    EstimatedTime,
    DueDate,
    StoryPoints,
    AssigneeId
)
{
    public override CreateBugfixProjectTaskCommand MapToCommand()
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
                AssigneeId is not null ? new UserId(AssigneeId.Value) : null
            ));

        return command;
    }
}