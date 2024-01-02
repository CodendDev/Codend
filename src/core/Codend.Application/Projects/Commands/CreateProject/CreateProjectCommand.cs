using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Shared.Infrastructure.Lexorank;
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
    private readonly IHttpContextProvider _contextProvider;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectCommandHandler"/> class.
    /// </summary>
    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IHttpContextProvider contextProvider,
        IProjectTaskStatusRepository statusRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _contextProvider = contextProvider;
        _statusRepository = statusRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var resultProject = Project.Create(userId, request.Name, request.Description);
        if (resultProject.IsFailed)
        {
            return resultProject.ToResult();
        }

        var project = resultProject.Value;

        // Create default statuses with positions.
        var defaultStatusesList = DefaultTaskStatus.SortedList;
        var statusesPositions = Lexorank.GetSpacedOutValuesBetween(defaultStatusesList.Count);
        var resultStatuses = defaultStatusesList
            .Select((status, i) => ProjectTaskStatus.Create(project.Id, status.Name, statusesPositions[i]))
            .ToList();
        var result = resultStatuses.Merge();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        // Set To-Do as project default status.
        var statuses = resultStatuses.Select(r => r.Value).ToList();
        var defaultStatus = statuses.FirstOrDefault(status => status.Name.Value == DefaultTaskStatus.ToDo.Name);
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
        await _statusRepository.AddRangeAsync(statuses);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id.Value;
    }
}