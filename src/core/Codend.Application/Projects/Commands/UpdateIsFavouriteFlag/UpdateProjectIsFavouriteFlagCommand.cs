using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.General;

namespace Codend.Application.Projects.Commands.UpdateIsFavouriteFlag;

/// <summary>
/// Command for updating IsFavourite project flag.
/// </summary>
/// <param name="ProjectId">Id of the project, which flag will be updated.</param>
/// <param name="IsFavourite">Is favourite flag.</param>
public sealed record UpdateProjectIsFavouriteFlagCommand(
        ProjectId ProjectId,
        bool IsFavourite)
    : ICommand;

/// <summary>
/// <see cref="UpdateProjectIsFavouriteFlagCommand"/> handler.
/// </summary>
public class UpdateProjectIsFavouriteFlagCommandHandler : ICommandHandler<UpdateProjectIsFavouriteFlagCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IHttpContextProvider _contextProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectIsFavouriteFlagCommandHandler"/> class.
    /// </summary>
    public UpdateProjectIsFavouriteFlagCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectMemberRepository memberRepository,
        IHttpContextProvider contextProvider)
    {
        _unitOfWork = unitOfWork;
        _memberRepository = memberRepository;
        _contextProvider = contextProvider;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateProjectIsFavouriteFlagCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextProvider.UserId;
        var projectMember =
            await _memberRepository.GetByProjectAndMemberId(request.ProjectId, userId, cancellationToken);
        if (projectMember is null)
        {
            return DomainNotFound.Fail<ProjectMember>();
        }

        var result = projectMember.SetIsFavourite(request.IsFavourite).ToResult();

        if (result.IsFailed)
        {
            return result;
        }

        _memberRepository.Update(projectMember);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}