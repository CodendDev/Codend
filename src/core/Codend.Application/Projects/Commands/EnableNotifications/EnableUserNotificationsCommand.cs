using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Projects.Commands.DisableNotifications;
using Codend.Domain.Core.Errors;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;

namespace Codend.Application.Projects.Commands.EnableNotifications;

/// <summary>
/// The command to enable user notification for a specific project.
/// </summary>
/// <param name="ProjectId">The id of the project to enable notifications for.</param>
public record EnableUserNotificationsCommand
(
    ProjectId ProjectId
) : ICommand;

/// <summary>
/// <see cref="DisableUserNotificationsCommand"/> handler.
/// </summary>
public class EnableUserNotificationsCommandHandler : ICommandHandler<EnableUserNotificationsCommand>
{
    private readonly IHttpContextProvider _context;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableUserNotificationsCommandHandler"/> class.
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
        var projectMember = await
            _memberRepository.GetByProjectAndMemberId(request.ProjectId, userId, cancellationToken);

        if (projectMember is null)
        {
            return DomainErrors.General.DomainNotFound.Fail<ProjectMember>();
        }

        var result = projectMember.EnableNotifications();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _memberRepository.Update(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}