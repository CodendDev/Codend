using Codend.Shared;

namespace Codend.Contracts.ProjectTasks;

public interface IUpdateProjectTaskRequest<out T>
{
    public Guid TaskId { get; }
    IShouldUpdate<string>? Name { get; }
    IShouldUpdate<string>? Priority { get; }
    IShouldUpdate<string?>? Description { get; }
    IShouldUpdate<DateTime?>? DueDate { get; }
    IShouldUpdate<uint?>? StoryPoints { get; }
    IShouldUpdate<Guid>? StatusId { get; }
    IShouldUpdate<IEstimatedTimeRequest>? EstimatedTime { get; }
    IShouldUpdate<Guid?>? AssigneeId { get; }
}