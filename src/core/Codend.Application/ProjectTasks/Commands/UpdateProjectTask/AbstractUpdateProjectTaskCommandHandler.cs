using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

/// <summary>
/// Abstract handler for updating any <see cref="AbstractProjectTask"/>. 
/// </summary>
/// <typeparam name="TCommand">
/// Command that implements <see cref="IUpdateProjectTaskCommand{TUpdateProjectTaskProperties}"/> interface.
/// </typeparam>
public abstract class AbstractUpdateProjectTaskCommandHandler<TCommand, TProjectTask, TUpdateProjectTaskProperties>
    : ICommandHandler<TCommand>
    where TCommand : ICommand, IUpdateProjectTaskCommand<TUpdateProjectTaskProperties>
    where TProjectTask : AbstractProjectTask
    where TUpdateProjectTaskProperties : AbstractProjectTaskUpdateProperties
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    protected AbstractUpdateProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
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

        var result = HandleUpdate(task, request.UpdateTaskProperties);
        if (result.IsFailed)
        {
            return result;
        }

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    protected virtual Result HandleUpdate(TProjectTask task, TUpdateProjectTaskProperties properties)
    {
        var results = new List<Result>();

        if (properties.Name.ShouldUpdate)
        {
            results.Add(task.EditName(properties.Name.Value!).ToResult());
        }

        if (properties.Priority.ShouldUpdate)
        {
            var priorityParsed = ProjectTaskPriority.TryFromName(properties.Priority.Value, true, out var priority);
            var resultPriority = priorityParsed
                ? Result.Ok(priority)
                : Result.Fail(new DomainErrors.ProjectTaskPriority.InvalidPriorityName());
            task.ChangePriority(priority);
            results.Add(resultPriority.ToResult());
        }

        if (properties.StatusId.ShouldUpdate)
        {
            results.Add(task.ChangeStatus(properties.StatusId.Value).ToResult());
        }

        if (properties.Description.ShouldUpdate)
        {
            results.Add(task.EditDescription(properties.Description.Value!).ToResult());
        }

        if (properties.EstimatedTime.ShouldUpdate)
        {
            results.Add(task.EditEstimatedTime(properties.EstimatedTime.Value).ToResult());
        }

        if (properties.DueDate.ShouldUpdate)
        {
            results.Add(task.SetDueDate(properties.DueDate.Value).ToResult());
        }

        if (properties.StoryPoints.ShouldUpdate)
        {
            results.Add(task.EditStoryPoints(properties.StoryPoints.Value).ToResult());
        }

        if (properties.AssigneeId.ShouldUpdate)
        {
            results.Add(task.AssignUser(properties.AssigneeId.Value).ToResult());
        }

        return Result.Merge(results.ToArray());
    }
}