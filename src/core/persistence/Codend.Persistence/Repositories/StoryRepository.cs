using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class StoryRepository : GenericRepository<StoryId, Guid, Story>, IStoryRepository
{
    public StoryRepository(CodendApplicationDbContext context) : base(context)
    {
    }
}