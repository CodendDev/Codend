using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Application.Extensions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;
using static Codend.Domain.Core.Errors.DomainErrors.General;
using static Codend.Domain.Core.Errors.DomainErrors.ProjectMember;

namespace Codend.Application.Projects.Commands.AddMember;

/// <summary>
/// Command to add member to a project.
/// </summary>
/// <param name="ProjectId">Project id.</param>
/// <param name="Userid">User id which will be added as member.</param>
public sealed record AddMemberCommand
(
    ProjectId ProjectId,
    UserId Userid
) : ICommand;

/// <summary>
/// <see cref="AddMemberCommand"/> validator.
/// </summary>
public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddMemberCommandValidator"/> class.
    /// </summary>
    public AddMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(AddMemberCommand.ProjectId)));

        RuleFor(x => x.Userid)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(AddMemberCommand.Userid)));
    }
}

/// <summary>
/// <see cref="AddMemberCommand"/> handler.
/// </summary>
public class AddMemberCommandHandler : ICommandHandler<AddMemberCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddMemberCommandHandler"/> class.
    /// </summary>
    public AddMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository)
    {
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return DomainNotFound.Fail<Project>();
        }

        if (await _projectMemberRepository.IsProjectMember(request.Userid, request.ProjectId, cancellationToken))
        {
            return Result.Fail(new UserIsProjectMemberAlready());
        }

        project.AddUserToProject(request.Userid);
        _projectRepository.Update(project);

        var result = ProjectMember.Create(request.ProjectId, request.Userid);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        _projectMemberRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}