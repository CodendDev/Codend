using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class SprintRepository : GenericRepository<SprintId, Guid, Sprint>, ISprintRepository
{
    public SprintRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}