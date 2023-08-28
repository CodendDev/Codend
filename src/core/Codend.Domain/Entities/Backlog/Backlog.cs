using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public class Backlog : Entity<BacklogId>
{
    private Backlog(BacklogId id) : base(id)
    {
    }

    public ProjectId ProjectId { get; set; }

    public virtual List<ProjectTask> ProjectTasks { get; set; }
}