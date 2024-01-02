using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using MediatR;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Command for creating <see cref="BaseProjectTask"/>.
/// </summary>
/// <param name="TaskProperties"><see cref="BaseProjectTaskCreateProperties"/> properties.</param>
/// <param name="SprintId">Id of the sprint to which epic will be assigned.</param>
public sealed record CreateBaseProjectTaskCommand
(
    BaseProjectTaskCreateProperties TaskProperties,
    SprintId? SprintId
) : ICommand<Guid>, ICreateProjectTaskCommand<BaseProjectTaskCreateProperties>;

/// <summary>
/// <see cref="CreateBaseProjectTaskCommand"/> handler.
/// </summary>
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
        IHttpContextProvider contextProvider,
        IProjectMemberRepository projectMemberRepository,
        IStoryRepository storyRepository,
        IProjectTaskStatusRepository statusRepository,
        IMediator mediator)
        : base(projectTaskRepository,
            unitOfWork,
            contextProvider,
            projectMemberRepository,
            storyRepository,
            statusRepository,
            mediator)
    {
    }
}