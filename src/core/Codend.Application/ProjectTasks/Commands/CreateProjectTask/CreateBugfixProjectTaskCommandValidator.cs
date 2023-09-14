using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Validator for <see cref="CreateBugfixProjectTaskCommand"/>.
/// </summary>
public class CreateBugfixProjectTaskCommandValidator
    : CreateProjectTaskCommandAbstractValidator<CreateBugfixProjectTaskCommand, BugfixProjectTaskCreateProperties>
{
}