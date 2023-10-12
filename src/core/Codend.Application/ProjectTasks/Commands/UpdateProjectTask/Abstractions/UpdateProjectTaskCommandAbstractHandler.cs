using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

/// <summary>
/// Abstract handler for updating any <see cref="BaseProjectTask"/> class. 
/// </summary>
/// <typeparam name="TCommand">
/// Command that implements <see cref="IUpdateProjectTaskCommand"/> interface.
/// </typeparam>
/// <typeparam name="TProjectTask">
/// ProjectTask derived from <see cref="BaseProjectTask"/> class.
/// </typeparam>
public abstract class UpdateProjectTaskCommandAbstractHandler<TCommand, TProjectTask>
    : ICommandHandler<TCommand>
    where TCommand : ICommand, IUpdateProjectTaskCommand
    where TProjectTask : BaseProjectTask
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IStoryRepository _storyRepository;

    /// <summary>
    /// Constructs implementation of <see cref="UpdateProjectTaskCommandAbstractHandler{TCommand,TProjectTask}"/> with
    /// <typeparamref name="TCommand"/> and <typeparamref name="TProjectTask"/> classes.
    /// </summary>
    /// <param name="taskRepository">Repository used for <see cref="BaseProjectTask"/>s.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="memberRepository">Repository for <see cref="ProjectMember"/>.</param>
    /// <param name="storyRepository">Repository for <see cref="Story"/></param>
    protected UpdateProjectTaskCommandAbstractHandler(
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IProjectMemberRepository memberRepository,
        IStoryRepository storyRepository)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _memberRepository = memberRepository;
        _storyRepository = storyRepository;
    }

    /// <summary>
    /// Handles command request. Calls <see cref="HandleUpdate"/> to update task.
    /// </summary>
    /// <param name="request">Command request.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // Validate task id.
        if (await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken) is not TProjectTask task)
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }

        // Validate status.
        if (request.StatusId is not null)
        {
            var statusExists = _taskRepository.ProjectTaskStatusIsValid(task.ProjectId, request.StatusId);
            if (!statusExists)
            {
                return Result.Fail(new InvalidStatusId());
            }
        }

        // Validate assignee.
        if (request.AssigneeId.ShouldUpdate)
        {
            var assigneeValid = await
                _memberRepository.IsProjectMember(request.AssigneeId.Value!, task.ProjectId, cancellationToken);
            if (!assigneeValid)
            {
                return Result.Fail(new InvalidAssigneeId());
            }
        }

        // Validate story.
        if (request.StoryId.ShouldUpdate)
        {
            var story = await _storyRepository.GetByIdAsync(request.StoryId.Value!, cancellationToken);
            if (story is null || story.ProjectId != task.ProjectId)
            {
                return Result.Fail(new InvalidStoryId());
            }
        }

        var result = HandleUpdate(task, request);
        if (result.IsFailed)
        {
            return result;
        }

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    /// <summary>
    /// Implementation of <see cref="HandleUpdate"/> for <see cref="BaseProjectTask"/>.
    /// Overwrite to create custom task updater.
    /// </summary>
    /// <param name="task">Task which will be updated.</param>
    /// <param name="request">Command request.</param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    protected virtual Result HandleUpdate(TProjectTask task, TCommand request)
    {
        var result = Result.Merge
        (
            request.Name.GetResultFromDelegate(task.EditName, Result.Ok),
            request.Description.HandleUpdateWithResult(task.EditDescription),
            request.EstimatedTime.HandleUpdate(task.EditEstimatedTime),
            request.DueDate.HandleUpdate(task.EditDueDate),
            request.StoryPoints.HandleUpdate(task.EditStoryPoints),
            request.AssigneeId.HandleUpdate(task.AssignUser),
            request.StoryId.HandleUpdate(task.EditStory),
            request.StatusId.GetResultFromDelegate(task.EditStatus, Result.Ok),
            request.Priority.GetResultFromDelegate(task.EditPriority, Result.Ok)
        );

        return result;
    }
}