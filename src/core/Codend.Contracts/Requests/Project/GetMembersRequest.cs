namespace Codend.Contracts.Requests.Project;

/// <summary>
/// Represents the get members request.
/// </summary>
/// <param name="Search">Text to search for inside user first name, last name or email.</param>
public record GetMembersRequest
(
    string? Search = null
);