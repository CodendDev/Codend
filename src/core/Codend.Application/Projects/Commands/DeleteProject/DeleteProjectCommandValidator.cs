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
    private readonly IProjectRepository _projectRepository;
    private readonly IUserIdentityProvider _identityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectCommandValidator"/> class.
    /// </summary>
    public DeleteProjectCommandValidator(IProjectRepository projectRepository,
        IUserIdentityProvider identityProvider)
    {
        _projectRepository = projectRepository;
        _identityProvider = identityProvider;

        RuleFor(x => x.ProjectId)
            .MustAsync(ProjectExistsAndUserIsOwner)
            .WithError(new ValidationErrors.Project.NotFoundOrUserUnauthorized())
            .WithSeverity(Severity.Warning);
    }

    private async Task<bool> ProjectExistsAndUserIsOwner(Guid projectId, CancellationToken cancellationToken)
    {
        var userId = _identityProvider.UserId;
        var project =
            await _projectRepository.GetByIdAsync(new ProjectId(projectId));

        return project is not null && project.OwnerId.Equals(userId);
    }
}