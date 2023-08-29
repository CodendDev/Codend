using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities.User;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public abstract class ProjectTask : Aggregate<ProjectTaskId>, ISoftDeletableEntity
{
    protected ProjectTask(ProjectTaskId id) : base(id)
    {
    }
    
    public ProjectTaskName Name { get; private set; }
    public ProjectTaskDescription? Description { get; private set; }
    public ProjectTaskPriority Priority { get; private set; }
    public ProjectTaskStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public UserId UserId { get; private set; }
    public BacklogId BacklogId { get; private set; }
    
    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }
}