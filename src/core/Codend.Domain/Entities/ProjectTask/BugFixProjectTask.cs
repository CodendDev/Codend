using Codend.Domain.Core.Enums;

namespace Codend.Domain.Entities;

public class BugFixProjectTask : ProjectTask
{
    private BugFixProjectTask(ProjectTaskId id) : base(id)
    {
    }

    public new static BugFixProjectTask Create(
        string name,
        UserId ownerId,
        ProjectTaskPriority priority,
        string status,
        ProjectId projectId,
        string? description = null,
        TimeSpan? estimatedTime = null,
        DateTime? dueDate = null,
        uint? storyPoints = null,
        UserId? assigneeId = null)
    {
        var task = new BugFixProjectTask(new ProjectTaskId(Guid.NewGuid()));
        ((ProjectTask)task).Create(
            name,
            ownerId,
            priority,
            status,
            projectId,
            description,
            estimatedTime,
            dueDate,
            storyPoints,
            assigneeId);

        return task;
    }
}