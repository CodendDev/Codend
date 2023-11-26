namespace Codend.Contracts.Requests.User;

/// <summary>
/// Represents update user request.
/// </summary>
/// <param name="FirstName">User first name.</param>
/// <param name="LastName">User last name.</param>
/// <param name="ImageUrl">User avatar url.</param>
public record UpdateUserRequest(string FirstName, string LastName, string ImageUrl);