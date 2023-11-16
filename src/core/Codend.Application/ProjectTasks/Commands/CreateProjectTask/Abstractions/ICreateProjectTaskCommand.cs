using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;

/// <summary>
/// Create ProjectTask command interface
/// </summary>
public interface ICreateProjectTaskCommand
{
}

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <typeparam name="TProjectTaskProperties">
/// Properties interface needed for task creation.
/// Must implement <see cref="IProjectTaskCreateProperties"/> interface.
/// </typeparam>
public interface ICreateProjectTaskCommand<out TProjectTaskProperties> : ICreateProjectTaskCommand
    where TProjectTaskProperties : IProjectTaskCreateProperties
{
    /// <summary>
    /// Properties used for creating ProjectTask.
    /// </summary>
    TProjectTaskProperties TaskProperties { get; }

    /// <summary>
    /// Id of the sprint to which epic will be assigned.
    /// </summary>
    SprintId? SprintId { get; }
}