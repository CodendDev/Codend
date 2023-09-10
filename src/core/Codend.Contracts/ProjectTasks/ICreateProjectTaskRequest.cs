namespace Codend.Contracts.ProjectTasks;

public interface ICreateProjectTaskRequest<T>
{
    string Name { get; }
    string Priority { get; }
    Guid StatusId { get; }
    Guid ProjectId { get; }
    string? Description { get; }
    IEstimatedTimeRequest? EstimatedTime { get; }
    DateTime? DueDate { get; }
    uint? StoryPoints { get; }
    Guid? AssigneeId { get; }
}