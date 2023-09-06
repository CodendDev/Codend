using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using ProjectNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectErrors.ProjectNotFound;

namespace Codend.Application.Projects.Queries.GetProjectById;

public sealed record GetProjectByIdQuery(
        Guid Id)
    : IQuery<Project>;

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, Project>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<Project>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(new ProjectId(request.Id));
        if (project is null)
        {
            return Result.Fail(new ProjectNotFound());
        }

        return Result.Ok(project);
    }
}