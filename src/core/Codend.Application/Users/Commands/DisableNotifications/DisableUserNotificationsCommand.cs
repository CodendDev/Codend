using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Users.Commands.DisableNotifications;

/// <summary>
/// Represents a command to disable user notifications.
/// </summary>
public record DisableUserNotificationsCommand : ICommand;

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
        var projectMembers = await _memberRepository.GetByMembersIdAsync(userId, cancellationToken);

        var result = projectMembers.Select(projectMember => projectMember.DisableNotifications()).Merge();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _memberRepository.UpdateRange(projectMembers);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}