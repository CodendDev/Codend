using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Infrastructure.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Codend.Infrastructure.Authorization;

/// <summary>
/// Handles authorization logic for project operations.
/// </summary>
internal sealed class ProjectOperationsAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, ProjectId>, IProjectOperationsAuthorizationHandler
{
    private readonly IUserIdentityProvider _identityProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectOperationsAuthorizationHandler"/> class.
    /// </summary>
    public ProjectOperationsAuthorizationHandler(
        IUserIdentityProvider identityProvider,
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _identityProvider = identityProvider;
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        ProjectId projectId)
    {
        // TODO: grant access to admin

        var userId = _identityProvider.UserId;

        switch (requirement.Name)
        {
            // User must be project member.
            case nameof(ProjectOperations.Edit):
                if (await IsUserProjectMember(userId, projectId))
                {
                    context.Succeed(requirement);
                }

                context.Fail();
                break;

            // User must be project owner.
            case nameof(ProjectOperations.Delete):
                if (await IsUserProjectOwner(userId, projectId))
                {
                    context.Succeed(requirement);
                }

                context.Fail();
                break;
            default:
                throw new ArgumentException("Unknown permission requirement.", nameof(requirement));
        }
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