using AutoMapper;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Queries.GetProjectById;

public sealed record GetProjectByIdQuery(
        Guid Id)
    : IQuery<ProjectResponse>;

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

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