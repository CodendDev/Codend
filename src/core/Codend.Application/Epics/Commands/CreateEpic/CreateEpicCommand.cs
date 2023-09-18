using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectErrors;

namespace Codend.Application.Epics.Commands.CreateEpic;

/// <summary>
/// Command user for creating epic. 
/// </summary>
/// <param name="Name">Epic name.</param>
/// <param name="Description">Epic description.</param>
/// <param name="ProjectId">Epic projectId.</param>
public sealed record CreateEpicCommand
(
    string Name,
    string Description,
    Guid ProjectId
) : ICommand<Guid>;

/// <summary>
/// <see cref="CreateEpicCommand"/> handler.
/// </summary>
public class CreateEpicCommandHandler : ICommandHandler<CreateEpicCommand, Guid>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEpicCommandHandler"/> class.
    /// </summary>
    public CreateEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateEpicCommand request, CancellationToken cancellationToken)
    {
        var projectId = new ProjectId(request.ProjectId);
        if (!await _projectRepository.Exists(projectId))
        {
            return Result.Fail(new ProjectNotFound());
        }

        var epicResult = Epic.Create(request.Name, request.Description, projectId);
        if (epicResult.IsFailed)
        {
            return epicResult.ToResult();
        }

        var epic = epicResult.Value;
        _epicRepository.Add(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(epic.Id.Value);
    }
}