using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public class User : Entity<UserId>
{
    public User(UserId id) : base(id)
    {
    }

    public virtual List<Project> ProjectsOwned { get; private set; }

    public virtual List<Project> ParticipatingInProjects  { get; private set; }
}