using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.ProjectTasks;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// CreateBugfixProjectTaskCommand implements <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/>
/// with properties needed for BugfixTask creation.
/// </summary>
/// <param name="TaskProperties">BugfixProjectTask properties.</param>
public sealed record CreateBugfixProjectTaskCommand(
        BugFixProjectTaskProperties TaskProperties
    )
    : ICommand<Guid>, ICreateProjectTaskCommand<BugFixProjectTaskProperties>;

public class CreateBugfixProjectTaskCommandHandler :
    CreateProjectTaskCommandHandler<
        CreateBugfixProjectTaskCommand,
        BugFixProjectTask,
        BugFixProjectTaskProperties>
{
    public CreateBugfixProjectTaskCommandHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider)
        : base(projectTaskRepository, unitOfWork, identityProvider)
    {
    }
}

public static class CreateBugfixRequestExtensions
{
    public static CreateBugfixProjectTaskCommand MapToCommand(this CreateBugfixRequest request)
    {
        var command = new CreateBugfixProjectTaskCommand(
            new BugFixProjectTaskProperties(
                request.Name,
                request.Priority,
                new ProjectTaskStatusId(request.StatusId),
                new ProjectId(request.ProjectId),
                request.Description,
                request.EstimatedTime is not null
                    ? new TimeSpan(
                        (int)request.EstimatedTime.Days,
                        (int)request.EstimatedTime.Hours,
                        (int)request.EstimatedTime.Minutes, 0)
                    : null,
                request.DueDate,
                request.StoryPoints,
                request.AssigneeId is not null ? new UserId(request.AssigneeId.Value) : null
            ));

        return command;
    }
}