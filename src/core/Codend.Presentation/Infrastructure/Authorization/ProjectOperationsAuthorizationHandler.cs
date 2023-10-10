using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Codend.Presentation.Infrastructure.Authorization;

/// <summary>
/// Handles authorization logic for project operations.
/// </summary>
internal sealed class ProjectOperationsAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly IHttpContextProvider _contextProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectOperationsAuthorizationHandler"/> class.
    /// </summary>
    public ProjectOperationsAuthorizationHandler(
        IHttpContextProvider contextProvider,
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _contextProvider = contextProvider;
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement)
    {
        var userId = _contextProvider.UserId;
        var projectId = _contextProvider.ProjectId;
        if (projectId is null)
        {
            context.Fail();
            _contextProvider.SetResponseStatusCode(StatusCodes.Status404NotFound);
            return;
        }
        
        switch (requirement.Name)
        {
            // User must be project member.
            case nameof(ProjectOperations.Member):
                if (await IsUserProjectMember(userId, projectId))
                {
                    context.Succeed(requirement);
                    return;
                }

                context.Fail();
                break;
            // User must be project owner.
            case nameof(ProjectOperations.Owner):
                if (await IsUserProjectOwner(userId, projectId))
                {
                    context.Succeed(requirement);
                    return;
                }

                context.Fail();
                break;
            default:
                throw new ArgumentException("Unknown permission requirement.", nameof(requirement));
        }
        _contextProvider.SetResponseStatusCode(StatusCodes.Status404NotFound);
    }

    private async Task<bool> IsUserProjectMember(UserId userId, ProjectId projectId)
    {
        return await _projectMemberRepository.IsProjectMember(userId, projectId, CancellationToken.None);
    }

    private async Task<bool> IsUserProjectOwner(UserId userId, ProjectId projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project is null)
        {
            return false;
        }

        return project.OwnerId == userId;
    }
}