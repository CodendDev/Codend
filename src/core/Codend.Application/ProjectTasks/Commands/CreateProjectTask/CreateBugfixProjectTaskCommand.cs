using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// CreateBugfixProjectTaskCommand implements <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/>
/// with properties needed for BugfixTask creation.
/// </summary>
/// <param name="TaskProperties">BugfixProjectTask properties.</param>
public sealed record CreateBugfixProjectTaskCommand(
        BugfixProjectTaskCreateProperties TaskProperties
    )
    : ICommand<Guid>, ICreateProjectTaskCommand<BugfixProjectTaskCreateProperties>;

public class CreateBugfixProjectTaskCommandHandler :
    CreateProjectTaskCommandAbstractHandler<
        CreateBugfixProjectTaskCommand,
        BugfixProjectTask,
        BugfixProjectTaskCreateProperties>
{
    public CreateBugfixProjectTaskCommandHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider)
        : base(projectTaskRepository, unitOfWork, identityProvider)
    {
    }
}