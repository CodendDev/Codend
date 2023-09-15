using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Record used for updating <see cref="BaseProjectTask"/> properties.
/// </summary>
public sealed record UpdateBaseProjectTaskCommand
(
    IShouldUpdate<string> Name,
    IShouldUpdate<string> Priority,
    IShouldUpdate<ProjectTaskStatusId> StatusId,
    IShouldUpdate<string?> Description,
    IShouldUpdate<TimeSpan?> EstimatedTime,
    IShouldUpdate<DateTime?> DueDate,
    IShouldUpdate<uint?> StoryPoints,
    IShouldUpdate<UserId?> AssigneeId,
    IShouldUpdate<StoryId?> StoryId
) : ICommand, IUpdateProjectTaskCommand
{
    /// <summary>
    /// Id of the project task that will be updated.
    /// </summary>
    public ProjectTaskId TaskId { get; set; }
}

/// <summary>
/// Command handler for <see cref="UpdateBaseProjectTaskCommand"/>.
/// </summary>
public class UpdateAbstractProjectTaskCommandHandler :
    UpdateProjectTaskCommandAbstractHandler<UpdateBaseProjectTaskCommand, BaseProjectTask>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAbstractProjectTaskCommandHandler"/> class.
    /// </summary>
    public UpdateAbstractProjectTaskCommandHandler(
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IProjectMemberRepository memberRepository,
        IProjectTaskStatusRepository projectTaskStatusRepository,
        IStoryRepository storyRepository,
        IUserIdentityProvider identityProvider)
        : base(taskRepository,
            unitOfWork,
            memberRepository,
            projectTaskStatusRepository,
            storyRepository,
            identityProvider)
    {
    }
}