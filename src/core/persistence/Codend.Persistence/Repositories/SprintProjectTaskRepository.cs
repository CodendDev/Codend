using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class SprintProjectTaskRepository
    : GenericRepository<SprintProjectTaskId, Guid, SprintProjectTask>,
        ISprintProjectTaskRepository
{
    public SprintProjectTaskRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}