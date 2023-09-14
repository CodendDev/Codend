using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Validator for <see cref="CreateBaseProjectTaskCommand"/>
/// </summary>
public class CreateBaseProjectTaskCommandValidator : CreateProjectTaskCommandAbstractValidator<
    CreateBaseProjectTaskCommand, BaseProjectTaskCreateProperties>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBaseProjectTaskCommandValidator"/> class.
    /// </summary>
    public CreateBaseProjectTaskCommandValidator(IUserIdentityProvider identityProvider,
        IProjectMemberRepository projectMemberRepository, IStoryRepository storyRepository,
        IProjectTaskStatusRepository projectTaskStatusRepository) : base(identityProvider, projectMemberRepository,
        storyRepository, projectTaskStatusRepository)
    {
    }
}