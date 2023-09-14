using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities.ProjectTask.Bugfix;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Validator for <see cref="CreateBugfixProjectTaskCommand"/>
/// </summary>
public class CreateBugfixProjectTaskCommandValidator : CreateProjectTaskCommandAbstractValidator<
    CreateBugfixProjectTaskCommand, BugfixProjectTaskCreateProperties>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBugfixProjectTaskCommandValidator"/> class.
    /// </summary>
    public CreateBugfixProjectTaskCommandValidator(IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository, IStoryRepository storyRepository,
        IProjectTaskStatusRepository projectTaskStatusRepository) : base(identityProvider, projectMemberRepository,
        storyRepository, projectTaskStatusRepository)
    {
    }
}