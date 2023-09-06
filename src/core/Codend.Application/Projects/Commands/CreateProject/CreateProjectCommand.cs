using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(
        string Name,
        string? Description)
    : ICommand<Guid>;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskStatusRepository _statusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider,
        IProjectTaskStatusRepository statusRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _statusRepository = statusRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var resultProject = Project.Create(_identityProvider.UserId, request.Name, request.Description);
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

        var statuses = resultStatuses.Select(r => r.Value);

        await _statusRepository.AddRangeAsync(statuses);
        _projectRepository.Add(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id.Value;
    }
}