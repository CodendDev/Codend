using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;

namespace Codend.Application.ProjectTasks.Commands.AssignUser;

/// <summary>
/// AssignUser to a ProjectTask command.
/// </summary>
public sealed record AssignUserCommand(
        Guid ProjectTaskId,
        Guid Assignee)
    : ICommand;

public class AssignUserCommandHandler : ICommandHandler<AssignUserCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignUserCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (task is null)
        {
            return Result.Fail(new ProjectTaskNotFound());
        }

        var result = task.AssignUser(new UserId(request.Assignee));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.ToResult();
    }
}