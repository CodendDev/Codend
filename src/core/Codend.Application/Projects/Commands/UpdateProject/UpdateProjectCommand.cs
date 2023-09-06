using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using ProjectNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectErrors.ProjectNotFound;

namespace Codend.Application.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
        Guid ProjectId,
        string Name,
        string? Description)
    : ICommand;

public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(new ProjectId(request.ProjectId));
        if (project is null)
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