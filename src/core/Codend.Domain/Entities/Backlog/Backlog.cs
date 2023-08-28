using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities.Backlog;

public class Backlog : Entity<BacklogId>
{
    public Backlog(BacklogId id) : base(id)
    {
    }
}
