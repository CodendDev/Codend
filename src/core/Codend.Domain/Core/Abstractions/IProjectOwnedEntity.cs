namespace Codend.Domain.Entities;

public interface IProjectOwnedEntity
{
    /// <summary>
    /// ProjectId which story belongs to.
    /// </summary>
    ProjectId ProjectId { get; }
}