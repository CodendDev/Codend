using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.ProjectTasks.Commands.DeleteProjectTask;

/// <summary>
/// Command to delete project task with given id.
/// </summary>
/// <param name="ProjectTaskId">Id of the task that will be deleted.</param>
public sealed record DeleteProjectTaskCommand(Guid ProjectTaskId) : ICommand;

/// <summary>
/// <see cref="DeleteProjectTaskCommand"/> handler.
/// </summary>
public class DeleteProjectTaskCommandHandler : ICommandHandler<DeleteProjectTaskCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectTaskCommandHandler"/> class.
    /// </summary>
    public DeleteProjectTaskCommandHandler(
        IProjectTaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _projectMemberRepository = projectMemberRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId), cancellationToken);
        if (projectTask is null)
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }
        
        // Validate user permission.
        var userId = _identityProvider.UserId;
        if (!await _projectMemberRepository.IsProjectMember(userId, projectTask.ProjectId, cancellationToken))
        {
            return DomainNotFound.Fail<BaseProjectTask>();
        }

        _taskRepository.Remove(projectTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}