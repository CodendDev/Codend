using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Errors;
using Codend.Application.Extensions;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;
using Codend.Domain.Repositories;
using Codend.Domain.ValueObjects;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Abstract validator for ProjectTask classes.
/// </summary>
public abstract class CreateProjectTaskCommandAbstractValidator<TCreateProjectTaskCommand, TCreateProjectTaskProperties>
    : AbstractValidator<TCreateProjectTaskCommand>
    where TCreateProjectTaskCommand : ICreateProjectTaskCommand<TCreateProjectTaskProperties>
    where TCreateProjectTaskProperties : IProjectTaskCreateProperties
{
    /// <summary>
    /// Identity provider, used to retrieve current user id.
    /// </summary>
    protected readonly IUserIdentityProvider IdentityProvider;

    /// <summary>
    /// ProjectMemberRepository used to check whether project exist and project members.
    /// </summary>
    protected readonly IProjectMemberRepository ProjectMemberRepository;

    /// <summary>
    /// StoryRepository used to check whether story exists.
    /// </summary>
    protected readonly IStoryRepository StoryRepository;

    /// <summary>
    /// ProjectTaskStatusRepository used to check whether status exists in project.
    /// </summary>
    protected readonly IProjectTaskStatusRepository ProjectTaskStatusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectTaskCommandAbstractValidator{TCreateProjectTaskCommand,TCreateProjectTaskProperties}"/> class.
    /// </summary>
    protected CreateProjectTaskCommandAbstractValidator(IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository, IStoryRepository storyRepository,
        IProjectTaskStatusRepository projectTaskStatusRepository)
    {
        IdentityProvider = identityProvider;
        ProjectMemberRepository = projectMemberRepository;
        StoryRepository = storyRepository;
        ProjectTaskStatusRepository = projectTaskStatusRepository;

        RuleFor(x => x.TaskProperties.ProjectId)
            .MustAsync(ProjectExistsAndUserIsMember)
            .WithError(new ValidationErrors.Project.NotFoundOrUserUnauthorized())
            .DependentRules(() =>
            {
                RuleFor(x => x.TaskProperties.Name)
                    .NotEmpty()
                    .WithError(new Common.PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.Name)))
                    .MaximumLength(ProjectTaskName.MaxLength)
                    .WithError(new Common.StringPropertyTooLong(nameof(IProjectTaskCreateProperties.Name),
                        ProjectTaskName.MaxLength));

                RuleFor(x => x.TaskProperties.Priority)
                    .NotEmpty()
                    .WithError(new Common.PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.Priority)))
                    .Must(x => ProjectTaskPriority.TryFromName(x, true, out _))
                    .WithError(new ProjectTask.PriorityNotDefined());

                RuleFor(x => x.TaskProperties.StatusId)
                    .NotEmpty()
                    .WithError(new Common.PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.StatusId)))
                    .MustAsync(StatusExistsInProject);

                RuleFor(x => x.TaskProperties.ProjectId)
                    .NotEmpty()
                    .WithError(new Common.PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.ProjectId)));

                RuleFor(x => x.TaskProperties.Description)
                    .NotEmpty()
                    .WithError(new Common.PropertyNullOrEmpty(nameof(IProjectTaskCreateProperties.Description)))
                    .MaximumLength(ProjectTaskDescription.MaxLength)
                    .WithError(new Common.StringPropertyTooLong(nameof(IProjectTaskCreateProperties.Description),
                        ProjectTaskDescription.MaxLength));

                RuleFor(x => x.TaskProperties.DueDate)
                    .Must(x => x.HasValue && x.Value.CompareTo(DateTime.UtcNow) > 0)
                    .WithError(new Common.DateIsInThePast(nameof(IProjectTaskCreateProperties.DueDate)));

                RuleFor(x => x.TaskProperties)
                    .MustAsync((x, cancellationToken) =>
                        AssigneeIsProjectMember(x.ProjectId, x.AssigneeId, cancellationToken))
                    .WithError(new ProjectTask.AssigneeNotExistOrIsNotProjectMember());

                RuleFor(x => x.TaskProperties.StoryId)
                    .MustAsync(StoryExists)
                    .WithError(new ValidationErrors.Story.NotFoundOrUserUnauthorized());
            });
    }

    /// <summary>
    /// Checks if given project exists, and current user permissions to this project.
    /// </summary>
    /// <returns>True - if project exists and user is member, False in other scenarios.</returns>
    protected async Task<bool> ProjectExistsAndUserIsMember(ProjectId projectId, CancellationToken cancellationToken)
    {
        var userId = IdentityProvider.UserId;
        var projectMember =
            await ProjectMemberRepository.GetByProjectAndMemberId(projectId, userId, cancellationToken);

        return projectMember is not null;
    }

    /// <summary>
    /// Checks whether status exists in project.
    /// </summary>
    /// <returns>True - if status exists, False - otherwise.</returns>
    protected async Task<bool> StatusExistsInProject(ProjectTaskStatusId statusId, CancellationToken cancellationToken)
    {
        var status = await ProjectTaskStatusRepository.GetByIdAsync(statusId);

        return status is not null;
    }

    /// <summary>
    /// Checks whether assignee is member of the project.
    /// </summary>
    /// <returns>True - if assignee is null or member, False - in other scenarios.</returns>
    protected async Task<bool> AssigneeIsProjectMember(ProjectId projectId, UserId? assigneeId,
        CancellationToken cancellationToken)
    {
        if (assigneeId is null)
        {
            return true;
        }

        var projectMember =
            await ProjectMemberRepository.GetByProjectAndMemberId(projectId, assigneeId, cancellationToken);

        return projectMember is not null;
    }

    /// <summary>
    /// Checks whether story with given id exits.
    /// </summary>
    /// <returns>True - if story is null or exists, False - in other scenarios.</returns>
    protected async Task<bool> StoryExists(StoryId? storyId, CancellationToken cancellationToken)
    {
        if (storyId is null)
        {
            return true;
        }

        var story = await StoryRepository.GetByIdAsync(storyId);

        return story is not null;
    }
}