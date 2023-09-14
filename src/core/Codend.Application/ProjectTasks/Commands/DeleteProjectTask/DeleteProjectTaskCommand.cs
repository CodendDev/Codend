using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using ProjectTaskNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors.ProjectTaskNotFound;

namespace Codend.Application.ProjectTasks.Commands.DeleteProjectTask;

/// <summary>
/// Command to delete project task with given id.
/// </summary>
/// <param name="ProjectTaskId">Id of the task that will be deleted.</param>
public sealed record DeleteProjectTaskCommand(Guid ProjectTaskId) : ICommand;

/// <inheritdoc />
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
        var projectTask = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (projectTask is null)
        {
            return Result.Fail(new ProjectTaskNotFound());
        }
        
        // Validate user permission.
        var userId = _identityProvider.UserId;
        if (!await _projectMemberRepository.IsProjectMember(userId, projectTask.ProjectId, cancellationToken))
        {
            return Result.Fail(new ProjectTaskNotFound());
        }

        _taskRepository.Remove(projectTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}