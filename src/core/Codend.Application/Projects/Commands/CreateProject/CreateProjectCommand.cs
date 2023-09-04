using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(
        string Name,
        string? Description)
    : ICommand<Project>;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityProvider _identityProvider;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserIdentityProvider identityProvider)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
    }

    public async Task<Result<Project>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var result = Project.Create(_identityProvider.UserId, request.Name, request.Description);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var project = result.Value;
        _projectRepository.Add(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}