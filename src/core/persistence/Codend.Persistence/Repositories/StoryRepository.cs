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
    public Task<List<Story>> GetStoriesByEpicIdAsync(EpicId epicId, CancellationToken cancellationToken)
    {
        var stories = Context.Set<Story>()
            .Where(story => story.EpicId == epicId)
            .ToListAsync(cancellationToken);

        return stories;
    }
    
    /// <inheritdoc />
    public Task<List<Story>> GetStoriesByStatusIdAsync(ProjectTaskStatusId statusId, CancellationToken cancellationToken)
    {
        var stories = Context.Set<Story>()
            .Where(story => story.StatusId == statusId)
            .ToListAsync(cancellationToken);

        return stories;
    }
}