using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

public sealed record CreateBugfixProjectTaskCommand(
        BugFixProjectTaskProperties TaskProperties
    )
    : ICommand<Guid>, ICreateProjectTaskCommand<BugFixProjectTaskProperties>;

public class CreateProjectTaskCommandHandler :
    CreateProjectTaskCommandHandler<
        CreateBugfixProjectTaskCommand,
        BugFixProjectTask,
        BugFixProjectTaskProperties>
{
    public CreateProjectTaskCommandHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider)
        : base(projectTaskRepository, unitOfWork, identityProvider)
    {
    }
}