using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;

namespace Codend.Presentation.Requests.ProjectTasks.Create.Abstractions;

public interface ICreateProjectTaskMapToCommand<out TCommand> where TCommand : ICreateProjectTaskCommand
{
    public TCommand MapToCommand();
}