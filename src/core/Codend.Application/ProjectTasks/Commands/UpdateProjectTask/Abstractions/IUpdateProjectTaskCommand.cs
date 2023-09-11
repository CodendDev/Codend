using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask;
using Codend.Domain.Entities.ProjectTask.Abstractions;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

public interface IUpdateProjectTaskCommand
{
}

public interface IUpdateProjectTaskCommand<out TUpdateProjectTaskProperties> : IUpdateProjectTaskCommand
    where TUpdateProjectTaskProperties : AbstractProjectTaskUpdateProperties
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