namespace Codend.Domain.Entities.ProjectTask.Abstractions;

/// <summary>
/// Interface used for creating any ProjectTask.
/// Contains all properties which are necessary for <see cref="BaseProjectTask"/> creation.
/// </summary>
public interface IProjectTaskCreateProperties
{
    ProjectId ProjectId { get; }
    public string Name { get; }
    public string Priority { get; }
    public ProjectTaskStatusId? StatusId { get; set; }
    string? Description { get; }
    TimeSpan? EstimatedTime { get; }
    DateTime? DueDate { get; }
    uint? StoryPoints { get; }
    UserId? AssigneeId { get; }
    StoryId? StoryId { get; }
}