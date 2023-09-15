using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;

namespace Codend.Application.ProjectTasks.Commands.AssignUser;

/// <summary>
/// Assign user to a ProjectTask command.
/// </summary>
public sealed record AssignUserCommand(
        Guid ProjectTaskId,
        Guid AssigneeId)
    : ICommand;

/// <summary>
/// <see cref="AssignUserCommand"/> handler.
/// </summary>
public class AssignUserCommandHandler : ICommandHandler<AssignUserCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserIdentityProvider _identityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignUserCommandHandler"/> class.
    /// </summary>
    public AssignUserCommandHandler(
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IProjectMemberRepository projectMemberRepository,
        IUserIdentityProvider identityProvider)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _projectMemberRepository = projectMemberRepository;
        _identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        // Validate task id.
        var task = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (task is null)
        {
            return Result.Fail(new ProjectTaskNotFound());
        }
        
        // Validate user permission.
        var userId = _identityProvider.UserId;
        if (!await _projectMemberRepository.IsProjectMember(userId, task.ProjectId, cancellationToken))
        {
            return Result.Fail(new ProjectTaskNotFound());
        }
        
        // Validate assignee id.
        var assigneeId = new UserId(request.AssigneeId);
        if (!await _projectMemberRepository.IsProjectMember(assigneeId, task.ProjectId, cancellationToken))
        {
            return Result.Fail(new InvalidAssigneeId());
        }

        var result = task.AssignUser(new UserId(request.AssigneeId));
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.ToResult();
    }
}