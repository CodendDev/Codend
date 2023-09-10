using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

public interface IUpdateProjectTaskCommand
{
}

public interface IUpdateProjectTaskCommand<out TUpdateProjectTaskProperties> : IUpdateProjectTaskCommand
    where TUpdateProjectTaskProperties : UpdateAbstractProjectTaskProperties
{
    /// <summary>
    /// Id of <see cref="AbstractProjectTask"/> which will be updated.
    /// </summary>
    ProjectTaskId TaskId { get; }

    /// <summary>
    /// Properties which will be applied on <see cref="AbstractProjectTask"/>.
    /// </summary>
    TUpdateProjectTaskProperties UpdateTaskProperties { get; }
}