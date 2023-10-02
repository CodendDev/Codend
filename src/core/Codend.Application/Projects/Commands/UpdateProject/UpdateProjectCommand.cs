using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Requests;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.Project;

namespace Codend.Application.Projects.Commands.UpdateProject;

/// <summary>
/// Command for updating properties in project with given id.
/// </summary>
/// <param name="ProjectId">Id of the project that will be updated.</param>
/// <param name="Name">New project name.</param>
/// <param name="Description">New project description.</param>
/// <param name="DefaultStatusId">New project default status.</param>
public sealed record UpdateProjectCommand(
        Guid ProjectId,
        string? Name,
        ShouldUpdateBinder<string?> Description,
        Guid? DefaultStatusId)
    : ICommand;

/// <summary>
/// <see cref="UpdateProjectCommand"/> handler.
/// </summary>
public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectCommandHandler"/> class.
    /// </summary>
    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork,
        IProjectMemberRepository projectMemberRepository, IUserIdentityProvider identityProvider, IProjectTaskStatusRepository statusRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _projectMemberRepository = projectMemberRepository;
        _identityProvider = identityProvider;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var projectId = request.ProjectId.GuidConversion<ProjectId>();
        var defaultStatusId = request.DefaultStatusId.GuidConversion<ProjectTaskStatusId>();
        
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (!await _projectMemberRepository.IsProjectMember(userId, project.Id, cancellationToken))
        {
            return DomainNotFound.Fail<Project>();
        }

        if (defaultStatusId != null &&
            !await _statusRepository
                .StatusExistsWithStatusIdAsync(defaultStatusId, projectId, cancellationToken))
        {
            return Result.Fail(new InvalidDefaultStatusId());
        }
        
        var result = Result.Merge(
            request.Name.GetResultFromDelegate(project.EditName, Result.Ok),
            request.Description.HandleUpdateWithResult(project.EditDescription),
            defaultStatusId.GetResultFromDelegate(project.EditDefaultStatus, Result.Ok));
        
        if (result.IsFailed)
        {
            return result;
        }

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}