using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask;

/// <summary>
/// Validator for <see cref="CreateBaseProjectTaskCommand"/>.
/// </summary>
public class CreateBaseProjectTaskCommandValidator
    : CreateProjectTaskCommandAbstractValidator<CreateBaseProjectTaskCommand, BaseProjectTaskCreateProperties>
{
}