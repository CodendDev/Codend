using Codend.Application.Core;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Contracts.Requests;
using Codend.Domain.Core.Extensions;
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
        ProjectId ProjectId,
        string? Name,
        ShouldUpdateBinder<string?> Description,
        ProjectTaskStatusId? DefaultStatusId)
    : ICommand;

/// <summary>
/// <see cref="UpdateProjectCommand"/> handler.
/// </summary>
public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTaskStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectCommandHandler"/> class.
    /// </summary>
    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IProjectTaskStatusRepository statusRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (request.DefaultStatusId != null &&
            await _statusRepository.StatusExistsWithStatusIdAsync(
                request.DefaultStatusId,
                request.ProjectId,
                cancellationToken) is false)
        {
            return Result.Fail(new InvalidDefaultStatusId());
        }

        var result = Result.Merge(
            request.Name.GetResultFromDelegate(project.EditName, Result.Ok),
            request.Description.HandleUpdateWithResult(project.EditDescription),
            request.DefaultStatusId.GetResultFromDelegate(project.EditDefaultStatus, Result.Ok)
        );

        if (result.IsFailed)
        {
            return result;
        }

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}