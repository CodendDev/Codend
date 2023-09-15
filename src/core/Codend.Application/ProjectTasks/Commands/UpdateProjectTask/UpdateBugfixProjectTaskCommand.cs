using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Record used for updating <see cref="BugfixProjectTask"/> properties.
/// </summary>
public sealed record UpdateBugfixProjectTaskCommand(
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
    public ProjectTaskId TaskId { get; init; }
}

/// <summary>
/// Command handler for <see cref="UpdateBugfixProjectTaskCommand"/>.
/// </summary>
public class UpdateBugfixProjectTaskCommandHandler :
    UpdateProjectTaskCommandAbstractHandler<UpdateBugfixProjectTaskCommand, BugfixProjectTask>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBugfixProjectTaskCommandHandler"/> class.
    /// </summary>
    public UpdateBugfixProjectTaskCommandHandler(
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