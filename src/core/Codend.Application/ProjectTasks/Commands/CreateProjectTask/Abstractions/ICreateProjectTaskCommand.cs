using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;

public interface ICreateProjectTaskCommand
{
}

/// <summary>
/// Create ProjectTask Command.
/// </summary>
/// <typeparam name="TProjectTaskProperties">
/// Properties interface needed for task creation.
/// Must implement <see cref="IProjectTaskCreateProperties"/> interface.
/// </typeparam>
public interface ICreateProjectTaskCommand<out TProjectTaskProperties> : ICreateProjectTaskCommand
    where TProjectTaskProperties : IProjectTaskCreateProperties
{
    TProjectTaskProperties TaskProperties { get; }
}