using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

public class CreateBugfixProjectTaskRequest : AbstractCreateProjectTaskRequest<CreateBugfixProjectTaskCommand>
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