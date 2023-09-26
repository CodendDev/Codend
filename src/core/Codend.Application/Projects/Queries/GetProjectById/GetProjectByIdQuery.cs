using AutoMapper;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Queries.GetProjectById;

/// <summary>
/// Command to retrieve project by it's id.
/// </summary>
/// <param name="Id">Id of the project to be retrieved.</param>
public sealed record GetProjectByIdQuery(
        Guid Id)
    : IQuery<ProjectResponse>;

/// <summary>
/// <see cref="GetProjectByIdQuery"/> Handler.
/// </summary>
public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectByIdQueryHandler"/> class.
    /// </summary>
    public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(new ProjectId(request.Id));
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        var dto = _mapper.Map<ProjectResponse>(project);
        return Result.Ok(dto);
    }
}