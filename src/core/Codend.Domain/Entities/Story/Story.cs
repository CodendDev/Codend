using Codend.Domain.Core.Abstractions;
using Codend.Domain.Core.Primitives;
using Codend.Domain.ValueObjects;

namespace Codend.Domain.Entities;

public class Story : Entity<StoryId>, ISoftDeletableEntity
{
    private Story(StoryId id) : base(id)
    {
    }

    public ProjectId ProjectId { get; set; }

    #region ISoftDeletableEntity properties

    public DateTime DeletedOnUtc { get; }
    public bool Deleted { get; }

    #endregion


    public StoryName Name { get; set; }
    public StoryDescription Description { get; set; }
}