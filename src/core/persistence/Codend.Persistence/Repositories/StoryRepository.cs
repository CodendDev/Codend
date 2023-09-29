using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codend.Persistence.Repositories;

public class StoryRepository : GenericRepository<StoryId, Guid, Story>, IStoryRepository
{
    public StoryRepository(CodendApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public Task<List<Story>> GetStoriesWithEpicId(EpicId epicId)
    {
        var stories = Context.Set<Story>()
            .Where(story => story.EpicId == epicId)
            .ToListAsync();

        return stories;
    }
    
    /// <inheritdoc />
    public Task<List<Story>> GetStoriesWithStatusId(ProjectTaskStatusId statusId)
    {
        var stories = Context.Set<Story>()
            .Where(story => story.StatusId == statusId)
            .ToListAsync();

        return stories;
    }
}