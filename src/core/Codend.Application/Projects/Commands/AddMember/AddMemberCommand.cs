using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Core.Abstractions.Services;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.Project;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectMember;

namespace Codend.Application.Projects.Commands.AddMember;

/// <summary>
/// Command to add member to a project.
/// </summary>
/// <param name="ProjectId">Project id.</param>
/// <param name="Email">User email which will be added as member if user exists.</param>
public sealed record AddMemberCommand
(
    ProjectId ProjectId,
    string Email
) : ICommand;

/// <summary>
/// <see cref="AddMemberCommand"/> handler.
/// </summary>
public class AddMemberCommandHandler : ICommandHandler<AddMemberCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddMemberCommandHandler"/> class.
    /// </summary>
    public AddMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository,
        IUserService userService
    )
    {
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (await _projectMemberRepository.GetProjectMembersCount(request.ProjectId) >= Project.MaxMembersCount)
        {
            return Result.Fail(new ProjectHasMaximumNumberOfMembers());
        }

        var user = await _userService.GetUserByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Ok();
        }

        if (
            await _projectMemberRepository
                .IsProjectMember(
                    user.Id.GuidConversion<UserId>(),
                    request.ProjectId,
                    cancellationToken
                )
        )
        {
            return Result.Fail(new UserIsProjectMemberAlready());
        }

        var userId = user.Id.GuidConversion<UserId>();
        project.AddUserToProject(userId);
        _projectRepository.Update(project);

        var result = ProjectMember.Create(request.ProjectId, userId);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _projectMemberRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}