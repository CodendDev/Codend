using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public abstract class ProjectTaskStatus : Entity<ProjectTaskStatusId>
{
    protected ProjectTaskStatus(ProjectTaskStatusId id) : base(id)
    {
    }

    public ProjectTaskStatusName Name { get; private set; }

    public ProjectId ProjectId { get; private set; }

    public virtual List<ProjectTask> ProjectTasks { get; private set; }
    
    public virtual Project Project { get; private set; }
}