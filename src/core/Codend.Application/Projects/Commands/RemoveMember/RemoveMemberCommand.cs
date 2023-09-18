using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Commands.RemoveMember;

/// <summary>
/// Command to remove member from a project.
/// </summary>
/// <param name="ProjectId">Project id.</param>
/// <param name="Userid">User id which will be removed as member.</param>
public sealed record RemoveMemberCommand
(
    ProjectId ProjectId,
    UserId Userid
) : ICommand;

/// <summary>
/// <see cref="RemoveMemberCommand"/> validator.
/// </summary>
public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveMemberCommandValidator"/> class.
    /// </summary>
    public RemoveMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RemoveMemberCommand.ProjectId)));

        RuleFor(x => x.Userid)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(RemoveMemberCommand.Userid)));
    }
}

/// <summary>
/// <see cref="RemoveMemberCommand"/> handler.
/// </summary>
public class RemoveMemberCommandHandler : ICommandHandler<RemoveMemberCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveMemberCommandHandler"/> class.
    /// </summary>
    public RemoveMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        // Check if relation exists
        var projectMember = await _projectMemberRepository.GetByUserAndProject(request.Userid, request.ProjectId);
        if (projectMember is null)
        {
            return DomainNotFound.Fail<ProjectMember>();
        }

        _projectMemberRepository.Remove(projectMember);

        // Update project
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        project.RemoveUserFromProject(request.Userid);
        _projectRepository.Update(project);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}