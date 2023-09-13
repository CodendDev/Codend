using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Codend.Domain.ValueObjects;
using FluentValidation;

namespace Codend.Application.Projects.Commands.UpdateProject;

/// <summary>
/// Validates update project command.
/// </summary>
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserIdentityProvider _identityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectCommandValidator"/> class.
    /// </summary>
    public UpdateProjectCommandValidator(IProjectMemberRepository projectMemberRepository,
        IUserIdentityProvider identityProvider)
    {
        _projectMemberRepository = projectMemberRepository;
        _identityProvider = identityProvider;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(new ValidationErrors.Common.StringPropertyNullOrEmpty(nameof(UpdateProjectCommand.Name)));

        RuleFor(x => x.Name)
            .MaximumLength(ProjectName.MaxLength)
            .WithError(new ValidationErrors.Common.StringPropertyTooLong(nameof(UpdateProjectCommand.Name),
                ProjectName.MaxLength));

        RuleFor(x => x.Description)
            .MaximumLength(ProjectDescription.MaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithError(new ValidationErrors.Common.StringPropertyTooLong(nameof(UpdateProjectCommand.Description),
                ProjectDescription.MaxLength));

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