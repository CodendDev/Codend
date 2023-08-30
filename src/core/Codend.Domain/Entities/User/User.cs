using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public class User : Entity<UserId>
{
    public User(UserId id) : base(id)
    {
    }
}