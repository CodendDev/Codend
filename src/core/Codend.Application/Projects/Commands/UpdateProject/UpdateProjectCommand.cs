using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using ProjectNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectErrors.ProjectNotFound;

namespace Codend.Application.Projects.Commands.UpdateProject;

/// <summary>
/// Command for updating properties in project with given id.
/// </summary>
/// <param name="ProjectId">Id of the project that will be updated.</param>
/// <param name="Name">New project name.</param>
/// <param name="Description">New project description.</param>
public sealed record UpdateProjectCommand(
        Guid ProjectId,
        string Name,
        string? Description)
    : ICommand;

/// <inheritdoc />
public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserIdentityProvider _identityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectCommandHandler"/> class.
    /// </summary>
    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork,
        IProjectMemberRepository projectMemberRepository, IUserIdentityProvider identityProvider)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _projectMemberRepository = projectMemberRepository;
        _identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var project = await _projectRepository.GetByIdAsync(new ProjectId(request.ProjectId));
        if (project is null)
        {
            return Result.Fail(new ProjectNotFound());
        }

        if (!await _projectMemberRepository.IsProjectMember(userId, project.Id, cancellationToken))
        {
            return Result.Fail(new ProjectNotFound());
        }
        
        var result = project.Edit(request.Name, request.Description);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.ToResult();
    }
}