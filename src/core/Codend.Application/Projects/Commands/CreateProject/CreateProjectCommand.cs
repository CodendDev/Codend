using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Projects.Commands.CreateProject;

/// <summary>
/// Command to create project with given properties.
/// </summary>
/// <param name="Name">Project Name.</param>
/// <param name="Description">Project Description.</param>
public sealed record CreateProjectCommand(
        string Name,
        string? Description)
    : ICommand<Guid>;

/// <summary>
/// <see cref="CreateProjectCommand"/> handler.
/// </summary>
public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectCommandHandler"/> class.
    /// </summary>
    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider,
        IProjectTaskStatusRepository statusRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _statusRepository = statusRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var resultProject = Project.Create(userId, request.Name, request.Description);
        if (resultProject.IsFailed)
        {
            return resultProject.ToResult();
        }

        var project = resultProject.Value;

        var resultStatuses = DefaultTaskStatus.List
            .Select(status => ProjectTaskStatus.Create(project.Id, status.Name))
            .ToList();
        var result = resultStatuses.Merge();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        // Set To-Do as first project default status.
        var defaultStatus = resultStatuses.FirstOrDefault(status => 
            status.Value.Name.Value == nameof(DefaultTaskStatus.ToDo))?.Value;
        if (defaultStatus is null)
        {
            throw new ApplicationException("Couldn't find default status for new Project.");
        }

        var resultProjectMember = ProjectMember.Create(project.Id, userId);
        if (resultProjectMember.IsFailed)
        {
            return resultProjectMember.ToResult();
        }
        
        _projectRepository.Add(project);
        _projectMemberRepository.Add(resultProjectMember.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Save statuses after project to avoid circular reference in database.
        project.EditDefaultStatus(defaultStatus.Id);
        var statuses = resultStatuses.Select(r => r.Value);
        await _statusRepository.AddRangeAsync(statuses);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id.Value;
    }
}