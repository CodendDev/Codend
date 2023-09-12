namespace Codend.Contracts.Responses.ProjectTask;

public record AbstractProjectTaskResponse
(
    string Name,
    string Priority,
    string? Description,
    DateTime? DueDate,
    uint? StoryPoints,
    Guid? AssigneeId,
    EstimatedTimeResponse? EstimatedTime
)
{
    public string Status { get; set; }
};