using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public class ProjectTaskStatus : Entity<ProjectTaskStatusId>
{
    public ProjectTaskStatus(ProjectTaskStatusId id) : base(id)
    {
    }

    public ProjectTaskStatusName Name { get; private set; }

    public ProjectId ProjectId { get; private set; }
}