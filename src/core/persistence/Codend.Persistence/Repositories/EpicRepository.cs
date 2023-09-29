using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class EpicRepository : GenericRepository<EpicId, Guid, Epic>, IEpicRepository
{
    public EpicRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public Task<List<Epic>> GetEpicsWithStatusId(ProjectTaskStatusId statusId)
    {
        var epics = Context.Set<Epic>()
            .Where(epic => epic.StatusId == statusId)
            .ToListAsync();

        return epics;
    }
}