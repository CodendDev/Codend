using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public abstract class ProjectTask : Aggregate<ProjectTaskId>, ISoftDeletableEntity
{
    protected ProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    public ProjectTaskName Name { get; private set; }
    public ProjectTaskDescription? Description { get; private set; }
    public ProjectTaskPriority Priority { get; private set; }
    public ProjectTaskStatusId StatusId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public UserId OwnerId { get; private set; }
    public UserId? AssigneeId { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public virtual List<Sprint> AssignedToSprints { get; private set; }
    public TimeSpan? EstimatedTime { get; private set; }
    public uint? StoryPoints { get; private set; }
}