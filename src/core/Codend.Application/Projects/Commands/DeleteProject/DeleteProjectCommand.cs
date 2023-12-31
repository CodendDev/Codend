using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Commands.DeleteProject;

/// <summary>
/// Command for deleting project with given id.
/// </summary>
/// <param name="ProjectId">Id of the project that will be deleted.</param>
public sealed record DeleteProjectCommand(
        ProjectId ProjectId)
    : ICommand;

/// <summary>
/// <see cref="DeleteProjectCommand"/> handler.
/// </summary>
public class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectCommandHandler"/> class.
    /// </summary>
    public DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        _projectRepository.Remove(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}