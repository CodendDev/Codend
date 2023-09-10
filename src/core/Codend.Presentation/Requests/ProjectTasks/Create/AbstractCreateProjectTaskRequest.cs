using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Contracts.ProjectTasks;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

public abstract record AbstractCreateProjectTaskRequest<TCommand>
(
    string Name,
    string Priority,
    Guid StatusId,
    Guid ProjectId,
    string? Description,
    EstimatedTimeRequest? _EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId
) : ICreateProjectTaskRequest
    where TCommand : ICreateProjectTaskCommand
{
    public IEstimatedTimeRequest? EstimatedTime => _EstimatedTime;
    public abstract TCommand MapToCommand();
}