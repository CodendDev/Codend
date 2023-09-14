using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

/// <summary>
/// Abstract handler for updating any <see cref="BaseProjectTask"/>. 
/// </summary>
/// <typeparam name="TCommand">
/// Command that implements <see cref="IUpdateProjectTaskCommand"/> interface.
/// </typeparam>
/// <typeparam name="TProjectTask">
/// Derived from AbstractProjectTask class.
/// </typeparam>
public abstract class UpdateProjectTaskCommandAbstractHandler<TCommand, TProjectTask>
    : ICommandHandler<TCommand>
    where TCommand : ICommand, IUpdateProjectTaskCommand
    where TProjectTask : BaseProjectTask
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructs implementation of <see cref="UpdateProjectTaskCommandAbstractHandler{TCommand,TProjectTask}"/> with
    /// <see cref="TCommand"/> and <see cref="TProjectTask"/> classes.
    /// </summary>
    /// <param name="taskRepository">Repository used for <see cref="BaseProjectTask"/>s.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    protected UpdateProjectTaskCommandAbstractHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles command request. Calls <see cref="HandleUpdate"/> to update task.
    /// </summary>
    /// <param name="request">Command request.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Result"/>.Ok() or a failure with errors.</returns>
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        if (await _taskRepository.GetByIdAsync(request.TaskId) is not TProjectTask task)
        {
            return Result.Fail(new ProjectTaskNotFound());
        }

        if (request.StatusId.ShouldUpdate)
        {
            var statusExists = _taskRepository.ProjectTaskStatusIsValid(task.ProjectId, request.StatusId.Value);
            if (!statusExists)
            {
                return Result.Fail(new InvalidStatusId());
            }
        }
        //TODO  asagnee

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
            request.Name.HandleUpdateWithResult(task.EditName),
            request.Description.HandleUpdateWithResult(task.EditDescription),
            request.EstimatedTime.HandleUpdate(task.EditEstimatedTime),
            request.DueDate.HandleUpdate(task.EditDueDate),
            request.StoryPoints.HandleUpdate(task.EditStoryPoints),
            request.AssigneeId.HandleUpdate(task.AssignUser),
            request.StoryId.HandleUpdate(task.EditStory),
            request.StatusId.HandleUpdate(task.EditStatus),
            request.Priority.HandleUpdateWithResult(task.EditPriority)
        );

        return result;
    }
}