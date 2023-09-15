using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class EpicRepository : GenericRepository<EpicId, Guid, Epic>, IEpicRepository
{
    public EpicRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}