using Codend.Application.ProjectTasks.Commands.CreateProjectTask;
using Codend.Application.ProjectTasks.Commands.CreateProjectTask.Abstractions;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks;

namespace Codend.Presentation.Requests.ProjectTasks.Create;

public abstract record AbstractCreateProjectTaskRequest<TCommand>
(
    string Name,
    string Priority,
    Guid StatusId,
    Guid ProjectId,
    string? Description,
    EstimatedTimeRequest? EstimatedTime,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId
) : ICreateProjectTaskRequest
    where TCommand : ICreateProjectTaskCommand
{
    public abstract TCommand MapToCommand();
}