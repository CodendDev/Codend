using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Users.Commands.DisableNotifications;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Users.Commands.EnableNotifications;

/// <summary>
/// Represents a command to enable user notifications.
/// </summary>
public record EnableUserNotificationsCommand : ICommand;

/// <summary>
/// <see cref="EnableUserNotificationsCommand"/> handler.
/// </summary>
public class EnableUserNotificationsCommandHandler : ICommandHandler<EnableUserNotificationsCommand>
{
    private readonly IHttpContextProvider _context;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnableUserNotificationsCommandHandler"/> class.
    /// </summary>
    public EnableUserNotificationsCommandHandler(
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
    public async Task<Result> Handle(EnableUserNotificationsCommand request, CancellationToken cancellationToken)
    {
        var userId = _context.UserId;
        var projectMembers = await _memberRepository.GetByMembersIdAsync(userId, cancellationToken);

        var result = projectMembers.Select(projectMember => projectMember.EnableNotifications()).Merge();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _memberRepository.UpdateRange(projectMembers);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}