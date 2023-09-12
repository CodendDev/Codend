using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Abstract handler for updating any <see cref="ProjectTaskBase"/>. 
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
    where TProjectTask : ProjectTaskBase
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    protected UpdateProjectTaskCommandAbstractHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        if (await _taskRepository.GetByIdAsync(request.TaskId) is not TProjectTask task)
        {
            return Result.Fail(new DomainErrors.ProjectTaskErrors.ProjectTaskNotFound());
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

    protected virtual Result HandleUpdate(TProjectTask task, TCommand request)
    {
        var results = new List<Result>();

        if (request.Name.ShouldUpdate)
        {
            results.Add(task.EditName(request.Name.Value!).ToResult());
        }

        if (request.Priority.ShouldUpdate)
        {
            var priorityParsed = ProjectTaskPriority.TryFromName(request.Priority.Value, true, out var priority);
            var resultPriority = priorityParsed
                ? Result.Ok(priority)
                : Result.Fail(new DomainErrors.ProjectTaskPriority.InvalidPriorityName());
            task.ChangePriority(priority);
            results.Add(resultPriority.ToResult());
        }

        if (request.StatusId.ShouldUpdate)
        {
            results.Add(task.ChangeStatus(request.StatusId.Value).ToResult());
        }

        if (request.Description.ShouldUpdate)
        {
            results.Add(task.EditDescription(request.Description.Value!).ToResult());
        }

        if (request.EstimatedTime.ShouldUpdate)
        {
            results.Add(task.EditEstimatedTime(request.EstimatedTime.Value).ToResult());
        }

        if (request.DueDate.ShouldUpdate)
        {
            results.Add(task.SetDueDate(request.DueDate.Value).ToResult());
        }

        if (request.StoryPoints.ShouldUpdate)
        {
            results.Add(task.EditStoryPoints(request.StoryPoints.Value).ToResult());
        }

        if (request.AssigneeId.ShouldUpdate)
        {
            results.Add(task.AssignUser(request.AssigneeId.Value).ToResult());
        }

        return Result.Merge(results.ToArray());
    }
}