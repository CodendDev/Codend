using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Projects.Commands.DisableNotifications;

/// <summary>
/// The command to disable user notification for a specific project.
/// </summary>
/// <param name="ProjectId">The id of the project to disable notifications for.</param>
public record DisableUserNotificationsCommand
(
    ProjectId ProjectId
) : ICommand;

/// <summary>
/// <see cref="DisableUserNotificationsCommand"/> handler.
/// </summary>
public class DisableUserNotificationsCommandHandler : ICommandHandler<DisableUserNotificationsCommand>
{
    private readonly IHttpContextProvider _context;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableUserNotificationsCommandHandler"/> class.
    /// </summary>
    public DisableUserNotificationsCommandHandler(
        IHttpContextProvider context,
        IProjectMemberRepository memberRepository,
        IUnitOfWork unitOfWork
    )
    {
        _context = context;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DisableUserNotificationsCommand request, CancellationToken cancellationToken)
    {
        var userId = _context.UserId;
        var projectMember = await
            _memberRepository.GetByProjectAndMemberId(request.ProjectId, userId, cancellationToken);

        if (projectMember is null)
        {
            return DomainErrors.General.DomainNotFound.Fail<ProjectMember>();
        }

        var result = projectMember.DisableNotifications();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _memberRepository.Update(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}