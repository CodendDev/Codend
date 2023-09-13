using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentValidation;

namespace Codend.Application.Projects.Commands.DeleteProject;

/// <summary>
/// Validates delete project command.
/// </summary>
public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserIdentityProvider _identityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectCommandValidator"/> class.
    /// </summary>
    public DeleteProjectCommandValidator(IProjectMemberRepository projectMemberRepository,
        IUserIdentityProvider identityProvider)
    {
        _projectMemberRepository = projectMemberRepository;
        _identityProvider = identityProvider;

        RuleFor(x => x.ProjectId)
            .MustAsync(ProjectExistsAndUserIsMember)
            .WithError(new ValidationErrors.Project.NotFoundOrUserUnauthorized());
    }

    private async Task<bool> ProjectExistsAndUserIsMember(Guid projectId, CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var projectMember =
            await _projectMemberRepository.GetByProjectAndMemberId(new ProjectId(projectId), userId, cancellationToken);

        return projectMember is not null;
    }
}