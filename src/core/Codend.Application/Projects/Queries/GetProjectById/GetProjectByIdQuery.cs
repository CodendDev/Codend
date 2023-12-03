using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Contracts.Responses.Project;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Queries.GetProjectById;

/// <summary>
/// Command to retrieve project by it's id.
/// </summary>
/// <param name="Id">Id of the project to be retrieved.</param>
public sealed record GetProjectByIdQuery
(
    ProjectId Id
) : IQuery<ProjectResponse>;

/// <summary>
/// <see cref="GetProjectByIdQuery"/> Handler.
/// </summary>
public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IHttpContextProvider _contextProvider;
    private readonly IQueryableSets _sets;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectByIdQueryHandler"/> class.
    /// </summary>
    public GetProjectByIdQueryHandler(
        IQueryableSets sets,
        IHttpContextProvider contextProvider
    )
    {
        _sets = sets;
        _contextProvider = contextProvider;
    }

    /// <inheritdoc />
    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var projectMember = _sets
            .Queryable<ProjectMember>()
            .Where(member =>
                member.MemberId == _contextProvider.UserId
            );

        var project = await _sets
            .Queryable<Project>()
            .Where(p =>
                p.Id == request.Id
            )
            .Join(projectMember,
                project => project.Id,
                member => member.ProjectId,
                (project, member) =>
                    new { project, member.IsFavourite, member.NotificationEnabled }
            )
            .SingleOrDefaultAsync(cancellationToken);

        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        return Result.Ok(
            new ProjectResponse(
                project.project.Id.Value,
                project.project.Name.Value,
                project.project.Description.Value,
                project.project.OwnerId.Value,
                project.IsFavourite,
                project.NotificationEnabled
            )
        );
    }
}