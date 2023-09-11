namespace Codend.Contracts.ProjectTasks;

public interface ICreateProjectTaskRequest
{
    string Name { get; }
    string Priority { get; }
    Guid StatusId { get; }
    Guid ProjectId { get; }
    string? Description { get; }
    EstimatedTimeRequest? EstimatedTime { get; }
    DateTime? DueDate { get; }
    uint? StoryPoints { get; }
    Guid? AssigneeId { get; }
}