using Codend.Domain.Entities;
using Codend.Domain.Repositories;

namespace Codend.Persistence.Repositories;

public class StoryRepository : GenericRepository<StoryId, Guid, Story>, IStoryRepository
{
    public StoryRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<Story> GetByEpicId(EpicId epicId)
    {
        var stories = Context.Set<Story>()
            .Where(story => story.EpicId == epicId);

        return stories;
    }
}