using Codend.Application.ProjectTasks.Commands.UpdateProjectTask.Abstractions;
using Codend.Contracts.Abstractions;
using Codend.Contracts.Requests;
using Codend.Contracts.Requests.ProjectTasks.Update;
// ReSharper disable InconsistentNaming

namespace Codend.Presentation.Requests.ProjectTasks.Update.Abstractions;

/// <summary>
/// Abstract implementation for <see cref="IUpdateBaseProjectTaskRequest"/> interface.
/// </summary>
/// <typeparam name="TCommand">Command implementing <see cref="IUpdateProjectTaskCommand"/>.</typeparam>
public abstract record UpdateProjectTaskAbstractRequest<TCommand>
(
    Guid TaskId,
    ShouldUpdateBinder<string>? _Name,
    ShouldUpdateBinder<string>? _Priority,
    ShouldUpdateBinder<string?>? _Description,
    ShouldUpdateBinder<DateTime?>? _DueDate,
    ShouldUpdateBinder<uint?>? _StoryPoints,
    ShouldUpdateBinder<Guid>? _StatusId,
    ShouldUpdateBinder<EstimatedTimeRequest>? _EstimatedTime,
    ShouldUpdateBinder<Guid?>? _AssigneeId,
    ShouldUpdateBinder<Guid?>? _StoryId
) : IUpdateBaseProjectTaskRequest
    where TCommand : IUpdateProjectTaskCommand
{
    /// <inheritdoc />
    public IShouldUpdate<string>? Name => _Name;

    /// <inheritdoc />
    public IShouldUpdate<string>? Priority => _Priority;

    /// <inheritdoc />
    public IShouldUpdate<string?>? Description => _Description;

    /// <inheritdoc />
    public IShouldUpdate<DateTime?>? DueDate => _DueDate;

    /// <inheritdoc />
    public IShouldUpdate<uint?>? StoryPoints => _StoryPoints;

    /// <inheritdoc />
    public IShouldUpdate<Guid>? StatusId => _StatusId;

    /// <inheritdoc />
    public IShouldUpdate<EstimatedTimeRequest>? EstimatedTime => _EstimatedTime;

    /// <inheritdoc />
    public IShouldUpdate<Guid?>? AssigneeId => _AssigneeId;

    /// <inheritdoc />
    public IShouldUpdate<Guid?>? StoryId => _StoryId;

    /// <summary>
    /// Method for mapping request to <see cref="TCommand"/>.
    /// </summary>
    /// <returns>Command created from request.</returns>
    public abstract TCommand MapToCommand();
}