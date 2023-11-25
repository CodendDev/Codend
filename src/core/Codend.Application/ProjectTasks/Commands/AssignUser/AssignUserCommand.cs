using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors;

namespace Codend.Application.ProjectTasks.Commands.AssignUser;

/// <summary>
/// Assign user to a ProjectTask command.
/// </summary>
public sealed record AssignUserCommand(
        ProjectTaskId ProjectTaskId,
        UserId AssigneeId)
    : ICommand;

/// <summary>
/// <see cref="AssignUserCommand"/> handler.
/// </summary>
public class AssignUserCommandHandler : ICommandHandler<AssignUserCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IHttpContextProvider _contextProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignUserCommandHandler"/> class.
    /// </summary>
    public AssignUserCommandHandler(
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IProjectMemberRepository projectMemberRepository,
        IHttpContextProvider contextProvider)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _projectMemberRepository = projectMemberRepository;
        _contextProvider = contextProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        // Validate task id.
        var task = await _taskRepository.GetByIdAsync(request.ProjectTaskId, cancellationToken);
        if (task is null)
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }

        // Validate user permission.
        var userId = _contextProvider.UserId;
        if (!await _projectMemberRepository.IsProjectMember(userId, task.ProjectId, cancellationToken))
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }
        
        // Validate assignee id.
        var member = await
            _projectMemberRepository.GetByProjectAndMemberId(task.ProjectId, request.AssigneeId, cancellationToken);
        if (member is null)
        {
            return Result.Fail(new InvalidAssigneeId());
        }

        var result = task.AssignUser(member);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.ToResult();
    }
}