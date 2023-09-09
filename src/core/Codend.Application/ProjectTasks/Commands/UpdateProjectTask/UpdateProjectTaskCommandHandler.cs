using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Errors;
using Codend.Domain.Repositories;
using FluentResults;
using InvalidPriorityName = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskPriority.InvalidPriorityName;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandHandler : ICommandHandler<UpdateProjectTaskCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        var results = new List<Result>();

        if (task is null)
        {
            return Result.Fail(new DomainErrors.ProjectTaskErrors.ProjectTaskNotFound());
        }

        if (request.Name.ShouldUpdate)
        {
            results.Add(task.EditName(request.Name.Value!).ToResult());
        }

        if (request.Priority.ShouldUpdate)
        {
            var priorityParsed = ProjectTaskPriority.TryFromName(request.Priority.Value, true, out var priority);
            var resultPriority = priorityParsed ? Result.Ok(priority) : Result.Fail(new InvalidPriorityName());
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
            results.Add(task.EditEstimatedTime(request.EstimatedTime.Value!.Value).ToResult());
        }

        if (request.DueDate.ShouldUpdate)
        {
            results.Add(task.SetDueDate(request.DueDate.Value!.Value).ToResult());
        }

        if (request.StoryPoints.ShouldUpdate)
        {
            results.Add(task.EditStoryPoints(request.StoryPoints.Value!.Value).ToResult());
        }

        if (request.AssigneeId.ShouldUpdate)
        {
            results.Add(task.AssignUser(request.AssigneeId.Value!).ToResult());
        }

        var result = Result.Merge(results.ToArray());
        if (result.IsFailed)
        {
            return result;
        }

        _taskRepository.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}