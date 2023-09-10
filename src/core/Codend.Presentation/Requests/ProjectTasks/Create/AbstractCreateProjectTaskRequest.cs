using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.ProjectTasks;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

public abstract class AbstractCreateProjectTaskRequest<TCommand> : ICreateProjectTaskRequest<TCommand>
    where TCommand : ICreateProjectTaskCommand
{
    public abstract TCommand MapToCommand();
    public string Name { get; init; }
    public string Priority { get; init; }
    public Guid StatusId { get; init; }
    public Guid ProjectId { get; init; }
    public string? Description { get; init; }

    public EstimatedTimeRequest? _EstimatedTime { get; init; }
    public IEstimatedTimeRequest? EstimatedTime => _EstimatedTime;
    public DateTime? DueDate { get; init; }
    public uint? StoryPoints { get; init; }
    public Guid? AssigneeId { get; init; }
}