using Codend.Domain.Entities;

namespace Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;

public interface IUpdateProjectTaskCommand
{
}

public interface IUpdateProjectTaskCommand<out TUpdateProjectTaskProperties> : IUpdateProjectTaskCommand
    where TUpdateProjectTaskProperties : UpdateAbstractProjectTaskProperties
{
    ProjectTaskId TaskId { get; }
    TUpdateProjectTaskProperties UpdateTaskProperties { get; }
}