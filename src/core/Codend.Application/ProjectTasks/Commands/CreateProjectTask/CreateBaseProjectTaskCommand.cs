using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// CreateBugfixProjectTaskCommand implements <see cref="ICreateProjectTaskCommand{TProjectTaskProperties}"/>
/// with properties needed for BugfixTask creation.
/// </summary>
/// <param name="TaskProperties">BugfixProjectTask properties.</param>
public sealed record CreateBaseProjectTaskCommand
(
    BaseProjectTaskCreateProperties TaskProperties
) : ICommand<Guid>, ICreateProjectTaskCommand<BaseProjectTaskCreateProperties>;

/// <inheritdoc />
public class CreateBaseProjectTaskCommandHandler :
    CreateProjectTaskCommandAbstractHandler<
        CreateBaseProjectTaskCommand,
        BaseProjectTask,
        BaseProjectTaskCreateProperties>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBaseProjectTaskCommandHandler"/> class.
    /// </summary>
    public CreateBaseProjectTaskCommandHandler(
        IProjectTaskRepository projectTaskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository,
        IStoryRepository storyRepository)
        : base(projectTaskRepository,
            unitOfWork,
            identityProvider,
            projectMemberRepository,
            storyRepository)
    {
    }
}