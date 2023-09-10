using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public sealed record UpdateBugfixProjectTaskCommand(
    ProjectTaskId TaskId,
    BugfixUpdateProjectTaskProperties UpdateTaskProperties
) : ICommand, IUpdateProjectTaskCommand<BugfixUpdateProjectTaskProperties>;

public class UpdateBugfixProjectTaskCommandHandler :
    AbstractUpdateProjectTaskCommandHandler<
        UpdateBugfixProjectTaskCommand,
        BugfixProjectTask,
        BugfixUpdateProjectTaskProperties>
{
    public UpdateBugfixProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
        : base(taskRepository, unitOfWork)
    {
    }
}