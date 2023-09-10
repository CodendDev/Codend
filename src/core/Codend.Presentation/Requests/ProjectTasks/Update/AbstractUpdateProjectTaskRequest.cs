using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.ProjectTasks;
using Codend.Shared;

namespace Codend.Presentation.Requests.ProjectTasks.Update;

/// <summary>
/// Abstract implementation for <see cref="IUpdateProjectTaskRequest{T}"/> interface.
/// </summary>
/// <typeparam name="TCommand">Command implementing <see cref="IUpdateProjectTaskCommand"/>.</typeparam>
public abstract record AbstractUpdateProjectTaskRequest<TCommand>
(
    Guid TaskId,
    ShouldUpdateBinder<string>? _Name,
    ShouldUpdateBinder<string>? _Priority,
    ShouldUpdateBinder<string?>? _Description,
    ShouldUpdateBinder<DateTime?>? _DueDate,
    ShouldUpdateBinder<uint?>? _StoryPoints,
    ShouldUpdateBinder<Guid>? _StatusId,
    ShouldUpdateBinder<EstimatedTimeRequest>? _EstimatedTime,
    ShouldUpdateBinder<Guid?>? _AssigneeId
) : IUpdateProjectTaskRequest<TCommand>
    where TCommand : IUpdateProjectTaskCommand
{
    public IShouldUpdate<string>? Name => _Name;
    public IShouldUpdate<string>? Priority => _Priority;
    public IShouldUpdate<string?>? Description => _Description;
    public IShouldUpdate<DateTime?>? DueDate => _DueDate;
    public IShouldUpdate<uint?>? StoryPoints => _StoryPoints;
    public IShouldUpdate<Guid>? StatusId => _StatusId;
    public IShouldUpdate<IEstimatedTimeRequest>? EstimatedTime => _EstimatedTime;
    public IShouldUpdate<Guid?>? AssigneeId => _AssigneeId;
    public abstract TCommand MapToCommand();
}